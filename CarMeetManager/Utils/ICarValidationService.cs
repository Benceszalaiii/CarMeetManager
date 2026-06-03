using CarMeetManager.Models;

namespace CarMeetManager.Utils
{
    public interface ICarValidationService
    {
        bool Validate(Car car, out string errorMessage);
    }
}
