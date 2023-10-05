using Api.Models;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;

namespace Api.Data
{
    public interface IEmployeeDataRepository
    {
        Task<List<Employee>> GetEmployeesWithCacheAsync();
    }

    // In a real-world scenario, I would establish a relational database, 
    // complete with properly defined tables and other database objects, 
    // to ensure optimized data storage and retrieval functionalities.
    public class EmployeeDataRepository : IEmployeeDataRepository
    {
        private readonly string _dataFilePath;
        private readonly IMemoryCache _memoryCache;
        private readonly IConfiguration _configuration;

        public EmployeeDataRepository(IConfiguration configuration, IMemoryCache memoryCache)
        {
            _configuration = configuration;
            _memoryCache = memoryCache;
            _dataFilePath = _configuration.GetValue<string>("EmployeeDataFilePath");

            if (string.IsNullOrEmpty(_dataFilePath) || !File.Exists(_dataFilePath))
            {
                throw new FileNotFoundException("Employee data file not found.", _dataFilePath);
            }
        }

        // This code is provided purely as an illustrative example,
        // it demonstrates an instance where local data caching might be 
        // considered to reduce database calls.
        public async Task<List<Employee>> GetEmployeesWithCacheAsync()
        {
            return await _memoryCache.GetOrCreateAsync("Employees", async entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = _configuration.GetValue<TimeSpan>("Cache:EmployeeExpiry");
                return await GetEmployeesAsync();
            });
        }

        private async Task<List<Employee>> GetEmployeesAsync()
        {
            string jsonData = await File.ReadAllTextAsync(_dataFilePath);

            if (string.IsNullOrWhiteSpace(jsonData))
                throw new InvalidDataException("Employee data file is empty.");

            try
            {
                return JsonConvert.DeserializeObject<List<Employee>>(jsonData);
            }
            catch (JsonException ex)
            {
                throw new InvalidDataException("Failed to deserialize employee data.", ex);
            }
        }
    }
}
