using System;
using System.Text;
using CarMeetManager.Models;

namespace CarMeetManager.Services
{
    public class CarValidationService : ICarValidationService
    {
        public bool Validate(Car car, out string errorMessage)
        {
            var builder = new StringBuilder();

            if (car == null)
            {
                builder.AppendLine("Car is required.");
            }
            else
            {
                if (string.IsNullOrWhiteSpace(car.Name))
                {
                    builder.AppendLine("Name is required.");
                }

                if (string.IsNullOrWhiteSpace(car.OwnerName))
                {
                    builder.AppendLine("Owner name is required.");
                }

                if (string.IsNullOrWhiteSpace(car.ParkingPlace))
                {
                    builder.AppendLine("Parking place is required.");
                }

                if (car.Year < 1900 || car.Year > DateTime.Now.Year + 1)
                {
                    builder.AppendLine("Year is not valid.");
                }

                if (car.Hp <= 0)
                {
                    builder.AppendLine("Horsepower must be greater than zero.");
                }

                if (car.Nm <= 0)
                {
                    builder.AppendLine("Torque must be greater than zero.");
                }

                if (car.Kg <= 0)
                {
                    builder.AppendLine("Weight must be greater than zero.");
                }

                if (!string.IsNullOrWhiteSpace(car.Image) && !Uri.IsWellFormedUriString(car.Image, UriKind.Absolute))
                {
                    builder.AppendLine("Image must be a valid URL.");
                }
            }

            errorMessage = builder.ToString().Trim();
            return string.IsNullOrEmpty(errorMessage);
        }
    }
}
