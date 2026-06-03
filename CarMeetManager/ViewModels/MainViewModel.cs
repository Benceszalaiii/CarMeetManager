using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using CarMeetManager.Models;
using CarMeetManager.Utils;

namespace CarMeetManager.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private readonly ICarStorageService _storageService;
        private readonly ICarValidationService _validationService;

        private ObservableCollection<Car> _cars;
        private Car _selectedCar;
        private string _searchText;
        private string _selectedParkingPlace;
        private string _errorMessage;

        public MainViewModel()
        {
            _storageService = new CarStorageService("Data/cars.json");
            _validationService = new CarValidationService();

            _cars = new ObservableCollection<Car>();
            ParkingPlaces = new ObservableCollection<string>();

            LoadCommand = new RelayCommand(OnLoad);
            SaveCommand = new RelayCommand(OnSave);
            AddCarCommand = new RelayCommand(OnAddCar);
            EditCarCommand = new RelayCommand(OnEditCar, CanEditOrDelete);
            DeleteCarCommand = new RelayCommand(OnDeleteCar, CanEditOrDelete);
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
            }
        }

        public string SelectedParkingPlace
        {
            get => _selectedParkingPlace;
            set
            {
                _selectedParkingPlace = value;
                OnPropertyChanged();
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

        private void OnLoad(object parameter)
        {
            throw new NotImplementedException();
        }

        private void OnSave(object parameter)
        {
            throw new NotImplementedException();
        }

        private void OnAddCar(object parameter)
        {
            throw new NotImplementedException();
        }

        private void OnEditCar(object parameter)
        {
            throw new NotImplementedException();
        }

        private void OnDeleteCar(object parameter)
        {
            throw new NotImplementedException();
        }

        private bool CanEditOrDelete(object parameter)
        {
            throw new NotImplementedException();
        }

        private void ShowError(string message)
        {
            throw new NotImplementedException();
        }
    }
}
