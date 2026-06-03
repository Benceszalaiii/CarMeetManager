using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using CarMeetManager.Models;
using CarMeetManager.Utils;

namespace CarMeetManager.ViewModels
{
    public class CarEditorViewModel : ViewModelBase
    {
        private readonly ICarValidationService _validationService;
        private Car _car;
        private string _errorMessage;

        public CarEditorViewModel()
        {
            _validationService = new CarValidationService();
            ParkingPlaces = new ObservableCollection<string>();

            SaveCommand = new RelayCommand(OnSave);
            CancelCommand = new RelayCommand(OnCancel);
        }

        public Car Car
        {
            get => _car;
            set
            {
                _car = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<string> ParkingPlaces { get; }

        public string ErrorMessage
        {
            get => _errorMessage;
            set
            {
                _errorMessage = value;
                OnPropertyChanged();
            }
        }

        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }

        private void OnSave(object parameter)
        {
            throw new NotImplementedException();
        }

        private void OnCancel(object parameter)
        {
            throw new NotImplementedException();
        }

        private bool ValidateCar()
        {
            throw new NotImplementedException();
        }
    }
}
