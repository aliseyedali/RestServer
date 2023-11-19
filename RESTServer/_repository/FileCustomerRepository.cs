using System.Text.Json;
using System.Xml;

namespace RESTServer
{
    public class FileCustomerRepository : ICustomerRepository
    {
        private const string JSONFileName = "customerData.json";
        private readonly string _dataFilePath;
        private readonly ILogger<FileCustomerRepository> _logger;
        private List<Customer> _customers = new List<Customer>();
        private readonly ReaderWriterLockSlim _lock = new ReaderWriterLockSlim();

        public FileCustomerRepository(ILogger<FileCustomerRepository> logger)
        {
            _dataFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, JSONFileName);
            _logger = logger;
            try
            {
                if (File.Exists(_dataFilePath))
                {
                    var data = File.ReadAllText(_dataFilePath);
                    _customers = System.Text.Json.JsonSerializer.Deserialize<List<Customer>>(data,
                        new System.Text.Json.JsonSerializerOptions
                        {
                            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                        });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Error reading customer data file", ex);
            }
        }

        public List<Customer> GetCustomers()
        {
            try
            {
                _lock.EnterReadLock();
                return _customers;
            }
            finally
            {
                _lock.ExitReadLock();
            }
        }

        public void InsertCustomer(int index, Customer newCustomer)
        {
            try
            {
                _lock.EnterWriteLock();
                _customers.Insert(index, newCustomer);
            }
            finally
            {
                _lock.ExitWriteLock();
            }
        }

        public void Save()
        {
            try
            {
                _lock.EnterWriteLock();
                var jsonData = System.Text.Json.JsonSerializer.Serialize(_customers
                    , new System.Text.Json.JsonSerializerOptions
                    {
                        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                    });
                File.WriteAllText(_dataFilePath, jsonData);
            }
            catch (Exception ex)
            {
                //_logger.LogError("Error saving customer data file", ex);
                throw new Exception($"Error saving customer data: {ex.Message}");
            }
            finally
            {
                _lock.ExitWriteLock();
            }
        }
    }
}
