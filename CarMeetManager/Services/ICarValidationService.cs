using CarMeetManager.Models;

namespace CarMeetManager.Services
{
    public interface ICarValidationService
    {
        bool Validate(Car car, out string errorMessage);
    }
}
