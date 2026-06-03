using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using CarMeetManager.Models;
using CarMeetManager.Services;

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
            if (!_validationService.Validate(Car, out var error))
            {
                ErrorMessage = error;
                return;
            }

            ErrorMessage = string.Empty;

            if (Application.Current.Windows.Count > 0)
            {
                var window = Application.Current.Windows[Application.Current.Windows.Count - 1];
                window.DialogResult = true;
                window.Close();
            }
        }

        private void OnCancel(object parameter)
        {
            if (Application.Current.Windows.Count > 0)
            {
                var window = Application.Current.Windows[Application.Current.Windows.Count - 1];
                window.DialogResult = false;
                window.Close();
            }
        }

        private bool ValidateCar()
        {
            if (!_validationService.Validate(Car, out var error))
            {
                ErrorMessage = error;
                return false;
            }

            ErrorMessage = string.Empty;
            return true;
        }
    }
}
