namespace RESTServer
{
    public interface ICutomerValidator
    {
        void Validate(Customer entity,ICustomerRepository repository);
    }
}
