namespace RESTServer
{
    public class CustomerService : ICustomerService
    {
        private readonly ILogger<CustomerService> _logger;
        private readonly ICustomerRepository _customerRepository;
        private readonly ICutomerValidator _customerValidator;

        public CustomerService(ILogger<CustomerService> logger
            , ICustomerRepository customerRepository
            , ICutomerValidator customerValidator)
        {
            _logger = logger;
            _customerRepository = customerRepository;
            _customerValidator = customerValidator;
        }

        public List<Customer> GetCustomers()
        {
            _logger.LogTrace("Get customers");
            return _customerRepository.GetCustomers();
        }

        public void AddCustomers(List<Customer> newCustomers)
        {
            _logger.LogTrace("Add customers");
            foreach (var customer in newCustomers)
            {
                _customerValidator.Validate(customer, _customerRepository);

                int index = _customerRepository.GetCustomers().BinarySearch(customer);
                if (index < 0)
                {
                    index = ~index;
                }

                _logger.LogDebug("Insert {FullName} at {Index}", customer.FullName, index);
                _customerRepository.InsertCustomer(index, customer);
            }

            _logger.LogTrace("save data changes");
            _customerRepository.Save();

        }
    }
}
