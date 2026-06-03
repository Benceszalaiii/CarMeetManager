using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
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

        public async Task<IList<Car>> LoadCarsAsync()
        {
            try
            {
                if (!File.Exists(_filePath))
                {
                    return new List<Car>();
                }

                var json = await File.ReadAllTextAsync(_filePath);
                if (string.IsNullOrWhiteSpace(json))
                {
                    return new List<Car>();
                }

                var cars = JsonSerializer.Deserialize<List<Car>>(json);
                return cars ?? new List<Car>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task SaveCarsAsync(IList<Car> cars)
        {
            try
            {
                var directory = Path.GetDirectoryName(_filePath);
                if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                var json = JsonSerializer.Serialize(cars, new JsonSerializerOptions
                {
                    WriteIndented = true
                });

                await File.WriteAllTextAsync(_filePath, json);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
