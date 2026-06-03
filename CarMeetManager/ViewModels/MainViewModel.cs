using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using CarMeetManager.Models;
using CarMeetManager.Services;
using CarMeetManager.Views;

namespace CarMeetManager.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private readonly ICarStorageService _storageService;
        private readonly ICarValidationService _validationService;

        private readonly List<Car> _allCars;

        private ObservableCollection<Car> _cars;
        private Car _selectedCar;
        private string _searchText;
        private string _selectedParkingPlace;
        private string _errorMessage;

        public MainViewModel()
        {
            _storageService = new CarStorageService("Data/cars.json");
            _validationService = new CarValidationService();

            _allCars = new List<Car>();
            _cars = new ObservableCollection<Car>();
            ParkingPlaces = new ObservableCollection<string>();

            LoadCommand = new RelayCommand(async _ => await OnLoadAsync());
            SaveCommand = new RelayCommand(async _ => await OnSaveAsync());
            AddCarCommand = new RelayCommand(_ => OnAddCar());
            EditCarCommand = new RelayCommand(_ => OnEditCar(), CanEditOrDelete);
            DeleteCarCommand = new RelayCommand(_ => OnDeleteCar(), CanEditOrDelete);

            _ = OnLoadAsync();
        }

        public ObservableCollection<Car> Cars
        {
            get => _cars;
            set
            {
                _cars = value;
                OnPropertyChanged();
            }
        }

        public Car SelectedCar
        {
            get => _selectedCar;
            set
            {
                _selectedCar = value;
                OnPropertyChanged();
                CommandManager.InvalidateRequerySuggested();
            }
        }

        public ObservableCollection<string> ParkingPlaces { get; }

        public string SearchText
        {
            get => _searchText;
            set
            {
                _searchText = value;
                OnPropertyChanged();
                ApplyFilters();
            }
        }

        public string SelectedParkingPlace
        {
            get => _selectedParkingPlace;
            set
            {
                _selectedParkingPlace = value;
                OnPropertyChanged();
                ApplyFilters();
            }
        }

        public string ErrorMessage
        {
            get => _errorMessage;
            set
            {
                _errorMessage = value;
                OnPropertyChanged();
            }
        }

        public ICommand LoadCommand { get; }
        public ICommand SaveCommand { get; }
        public ICommand AddCarCommand { get; }
        public ICommand EditCarCommand { get; }
        public ICommand DeleteCarCommand { get; }

        private async Task OnLoadAsync()
        {
            try
            {
                ErrorMessage = string.Empty;
                var cars = await _storageService.LoadCarsAsync();

                _allCars.Clear();
                _allCars.AddRange(cars);

                ApplyFilters();

                ParkingPlaces.Clear();
                foreach (var place in _allCars.Select(c => c.ParkingPlace).Distinct().OrderBy(p => p))
                {
                    if (!string.IsNullOrWhiteSpace(place))
                    {
                        ParkingPlaces.Add(place);
                    }
                }
            }
            catch (Exception ex)
            {
                ShowError("Failed to load cars: " + ex.Message);
            }
        }

        private async Task OnSaveAsync()
        {
            try
            {
                ErrorMessage = string.Empty;
                await _storageService.SaveCarsAsync(_allCars.ToList());
            }
            catch (Exception ex)
            {
                ShowError("Failed to save cars: " + ex.Message);
            }
        }

        private void OnAddCar()
        {
            ErrorMessage = string.Empty;

            var car = new Car
            {
                Year = DateTime.Now.Year,
                Hp = 100,
                Nm = 100,
                Kg = 1000
            };

            var editor = new CarEditorWindow
            {
                Owner = Application.Current.MainWindow
            };

            var vm = editor.DataContext as CarEditorViewModel;
            if (vm != null)
            {
                vm.Car = car;
                vm.ParkingPlaces.Clear();
                foreach (var place in ParkingPlaces)
                {
                    vm.ParkingPlaces.Add(place);
                }

                vm.NewParkingPlace = car.ParkingPlace;
            }

            editor.ShowDialog();

            if (vm != null && vm.IsSaved)
            {
                _allCars.Add(car);
                ApplyFilters();

                if (!string.IsNullOrWhiteSpace(car.ParkingPlace) && !ParkingPlaces.Contains(car.ParkingPlace))
                {
                    ParkingPlaces.Add(car.ParkingPlace);
                }
            }
        }

        private void OnEditCar()
        {
            if (SelectedCar == null)
            {
                return;
            }

            ErrorMessage = string.Empty;

            var carCopy = new Car
            {
                Image = SelectedCar.Image,
                Name = SelectedCar.Name,
                Year = SelectedCar.Year,
                OwnerName = SelectedCar.OwnerName,
                Hp = SelectedCar.Hp,
                Nm = SelectedCar.Nm,
                Kg = SelectedCar.Kg,
                ParkingPlace = SelectedCar.ParkingPlace
            };

            var editor = new CarEditorWindow
            {
                Owner = Application.Current.MainWindow
            };

            var vm = editor.DataContext as CarEditorViewModel;
            if (vm != null)
            {
                vm.Car = carCopy;
                vm.ParkingPlaces.Clear();
                foreach (var place in ParkingPlaces)
                {
                    vm.ParkingPlaces.Add(place);
                }

                vm.NewParkingPlace = carCopy.ParkingPlace;
            }

            editor.ShowDialog();

            if (vm != null && vm.IsSaved)
            {
                SelectedCar.Image = carCopy.Image;
                SelectedCar.Name = carCopy.Name;
                SelectedCar.Year = carCopy.Year;
                SelectedCar.OwnerName = carCopy.OwnerName;
                SelectedCar.Hp = carCopy.Hp;
                SelectedCar.Nm = carCopy.Nm;
                SelectedCar.Kg = carCopy.Kg;
                SelectedCar.ParkingPlace = carCopy.ParkingPlace;

                if (!string.IsNullOrWhiteSpace(SelectedCar.ParkingPlace) && !ParkingPlaces.Contains(SelectedCar.ParkingPlace))
                {
                    ParkingPlaces.Add(SelectedCar.ParkingPlace);
                }

                ApplyFilters();
            }
        }

        private void OnDeleteCar()
        {
            if (SelectedCar == null)
            {
                return;
            }

            ErrorMessage = string.Empty;

            _allCars.Remove(SelectedCar);
            ApplyFilters();

            var usedPlaces = _allCars.Select(c => c.ParkingPlace).Distinct().ToList();
            for (int i = ParkingPlaces.Count - 1; i >= 0; i--)
            {
                if (!usedPlaces.Contains(ParkingPlaces[i]))
                {
                    ParkingPlaces.RemoveAt(i);
                }
            }

            SelectedCar = null;
        }

        private bool CanEditOrDelete(object parameter)
        {
            return SelectedCar != null;
        }

        private void ApplyFilters()
        {
            if (_allCars == null)
            {
                return;
            }

            var query = _allCars.AsEnumerable();

            if (!string.IsNullOrWhiteSpace(SearchText))
            {
                var search = SearchText.Trim().ToLowerInvariant();
                query = query.Where(c =>
                    (!string.IsNullOrEmpty(c.Name) && c.Name.ToLowerInvariant().Contains(search)) ||
                    (!string.IsNullOrEmpty(c.OwnerName) && c.OwnerName.ToLowerInvariant().Contains(search)) ||
                    (!string.IsNullOrEmpty(c.ParkingPlace) && c.ParkingPlace.ToLowerInvariant().Contains(search)));
            }

            if (!string.IsNullOrWhiteSpace(SelectedParkingPlace))
            {
                query = query.Where(c => string.Equals(c.ParkingPlace, SelectedParkingPlace, StringComparison.OrdinalIgnoreCase));
            }

            Cars = new ObservableCollection<Car>(query);
        }

        private void ShowError(string message)
        {
            ErrorMessage = message;
        }
    }
}
