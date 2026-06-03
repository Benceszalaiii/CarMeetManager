using System.Collections.Generic;
using System.Threading.Tasks;
using CarMeetManager.Models;

namespace CarMeetManager.Utils
{
    public interface ICarStorageService
    {
        Task<IList<Car>> LoadCarsAsync();
        Task SaveCarsAsync(IList<Car> cars);
    }
}
