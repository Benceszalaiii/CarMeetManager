using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CarMeetManager.Models;

namespace CarMeetManager.Services
{
    public class CarStorageService : ICarStorageService
    {
        private readonly string _filePath;

        public CarStorageService(string filePath)
        {
            _filePath = filePath;
        }

        public Task<IList<Car>> LoadCarsAsync()
        {
            throw new NotImplementedException();
        }

        public Task SaveCarsAsync(IList<Car> cars)
        {
            throw new NotImplementedException();
        }
    }
}
