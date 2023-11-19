namespace RESTServer
{
    public interface ICustomerService
    {
        void AddCustomers(List<Customer> newCustomers);
        List<Customer> GetCustomers();
    }
}