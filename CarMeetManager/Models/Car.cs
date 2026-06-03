using System.ComponentModel;

namespace CarMeetManager.Models
{
    public class Car : INotifyPropertyChanged
    {
        private string _image;
        private string _name;
        private int _year;
        private string _ownerName;
        private int _hp;
        private int _nm;
        private int _kg;
        private string _parkingPlace;

        public string Image
        {
            get => _image;
            set
            {
                if (_image != value)
                {
                    _image = value;
                    OnPropertyChanged(nameof(Image));
                }
            }
        }

        public string Name
        {
            get => _name;
            set
            {
                if (_name != value)
                {
                    _name = value;
                    OnPropertyChanged(nameof(Name));
                }
            }
        }

        public int Year
        {
            get => _year;
            set
            {
                if (_year != value)
                {
                    _year = value;
                    OnPropertyChanged(nameof(Year));
                }
            }
        }

        public string OwnerName
        {
            get => _ownerName;
            set
            {
                if (_ownerName != value)
                {
                    _ownerName = value;
                    OnPropertyChanged(nameof(OwnerName));
                }
            }
        }

        public int Hp
        {
            get => _hp;
            set
            {
                if (_hp != value)
                {
                    _hp = value;
                    OnPropertyChanged(nameof(Hp));
                }
            }
        }

        public int Nm
        {
            get => _nm;
            set
            {
                if (_nm != value)
                {
                    _nm = value;
                    OnPropertyChanged(nameof(Nm));
                }
            }
        }

        public int Kg
        {
            get => _kg;
            set
            {
                if (_kg != value)
                {
                    _kg = value;
                    OnPropertyChanged(nameof(Kg));
                }
            }
        }

        public string ParkingPlace
        {
            get => _parkingPlace;
            set
            {
                if (_parkingPlace != value)
                {
                    _parkingPlace = value;
                    OnPropertyChanged(nameof(ParkingPlace));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
