using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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

        private readonly List<Car> _allCars = new List<Car>();

        private ObservableCollection<Car> _cars;
        private Car _selectedCar;
        private string _searchText;
        private string _selectedParkingPlace;
        private string _errorMessage;

        public const string AllParkingPlaces = "(All)";

        public MainViewModel()
        {
            _storageService = new CarStorageService("Data/cars.json");
            _validationService = new CarValidationService();

            _cars = new ObservableCollection<Car>();
            ParkingPlaces = new ObservableCollection<string> { AllParkingPlaces };

            LoadCommand = new RelayCommand(_ => OnLoad());
            SaveCommand = new RelayCommand(_ => OnSave());
            AddCarCommand = new RelayCommand(_ => OnAddCar());
            EditCarCommand = new RelayCommand(_ => OnEditCar(), CanEditOrDelete);
            DeleteCarCommand = new RelayCommand(_ => OnDeleteCar(), CanEditOrDelete);

            OnLoad();
        }

        public ObservableCollection<Car> Cars
        {
            get => _cars;
            private set
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

        private void OnLoad()
        {
            try
            {
                ErrorMessage = string.Empty;
                var cars = _storageService.LoadCars();

                _allCars.Clear();
                _allCars.AddRange(cars);

                RefreshParkingPlaces();
                ApplyFilters();
            }
            catch (Exception ex)
            {
                ShowError("Failed to load cars: " + ex.Message);
            }
        }

        private void OnSave()
        {
            try
            {
                ErrorMessage = string.Empty;
                _storageService.SaveCars(_allCars);
            }
            catch (Exception ex)
            {
                ShowError("Failed to save cars: " + ex.Message);
            }
        }

        private void OnAddCar()
        {
            ErrorMessage = string.Empty;

            var car = new Car { Year = DateTime.Now.Year, Hp = 100, Nm = 100, Kg = 1000 };

            var editor = new CarEditorWindow { Owner = Application.Current.MainWindow };
            var vm = editor.DataContext as CarEditorViewModel;

            if (vm != null)
            {
                vm.Car = car;
                SyncParkingPlacesToEditor(vm);
            }

            editor.ShowDialog();

            if (vm != null && vm.IsSaved)
            {
                _allCars.Add(car);
                RefreshParkingPlaces();
                ApplyFilters();
            }
        }

        private void OnEditCar()
        {
            if (SelectedCar == null) return;
            ErrorMessage = string.Empty;

            var snapshot = CopyCar(SelectedCar);
            var editor = new CarEditorWindow { Owner = Application.Current.MainWindow };
            var vm = editor.DataContext as CarEditorViewModel;

            if (vm != null)
            {
                vm.Car = snapshot;
                SyncParkingPlacesToEditor(vm);
            }

            editor.ShowDialog();

            if (vm != null && vm.IsSaved)
            {
                ApplyCopy(snapshot, SelectedCar);
                RefreshParkingPlaces();
                ApplyFilters();
            }
        }

        private void OnDeleteCar()
        {
            if (SelectedCar == null) return;
            ErrorMessage = string.Empty;

            _allCars.Remove(SelectedCar);
            SelectedCar = null;

            RefreshParkingPlaces();
            ApplyFilters();
        }

        private bool CanEditOrDelete(object _) => SelectedCar != null;

        private void ApplyFilters()
        {
            var query = _allCars.AsEnumerable();

            if (!string.IsNullOrWhiteSpace(SearchText))
            {
                var term = SearchText.Trim().ToLowerInvariant();
                query = query.Where(c =>
                    (c.Name ?? string.Empty).ToLowerInvariant().Contains(term) ||
                    (c.OwnerName ?? string.Empty).ToLowerInvariant().Contains(term) ||
                    (c.ParkingPlace ?? string.Empty).ToLowerInvariant().Contains(term));
            }

            if (!string.IsNullOrWhiteSpace(SelectedParkingPlace) && SelectedParkingPlace != AllParkingPlaces)
            {
                query = query.Where(c =>
                    string.Equals(c.ParkingPlace, SelectedParkingPlace, StringComparison.OrdinalIgnoreCase));
            }

            Cars = new ObservableCollection<Car>(query);
        }

        private void RefreshParkingPlaces()
        {
            var current = SelectedParkingPlace;

            ParkingPlaces.Clear();
            ParkingPlaces.Add(AllParkingPlaces);

            foreach (var place in _allCars
                .Select(c => c.ParkingPlace)
                .Where(p => !string.IsNullOrWhiteSpace(p))
                .Distinct()
                .OrderBy(p => p))
            {
                ParkingPlaces.Add(place);
            }

            SelectedParkingPlace = ParkingPlaces.Contains(current) ? current : AllParkingPlaces;
        }

        private void SyncParkingPlacesToEditor(CarEditorViewModel vm)
        {
            vm.ParkingPlaces.Clear();
            foreach (var place in ParkingPlaces.Where(p => p != AllParkingPlaces))
                vm.ParkingPlaces.Add(place);
        }

        private void ShowError(string message) => ErrorMessage = message;

        private static Car CopyCar(Car src) => new Car
        {
            Image = src.Image, Name = src.Name, Year = src.Year,
            OwnerName = src.OwnerName, Hp = src.Hp, Nm = src.Nm,
            Kg = src.Kg, ParkingPlace = src.ParkingPlace
        };

        private static void ApplyCopy(Car src, Car dst)
        {
            dst.Image = src.Image; dst.Name = src.Name; dst.Year = src.Year;
            dst.OwnerName = src.OwnerName; dst.Hp = src.Hp; dst.Nm = src.Nm;
            dst.Kg = src.Kg; dst.ParkingPlace = src.ParkingPlace;
        }
    }
}
