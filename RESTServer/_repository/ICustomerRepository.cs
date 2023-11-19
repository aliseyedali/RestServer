namespace RESTServer
{
    public interface ICustomerRepository
    {
        List<Customer> GetCustomers();

        void InsertCustomer(int index, Customer newCustomer);

        void Save();
    }
}
