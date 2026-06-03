using System.Collections.Generic;
using System.Threading.Tasks;
using CarMeetManager.Models;

namespace CarMeetManager.Services
{
    public interface ICarStorageService
    {
        Task<IList<Car>> LoadCarsAsync();
        Task SaveCarsAsync(IList<Car> cars);
    }
}
