using System.ComponentModel.DataAnnotations;

namespace RESTServer
{
    public class CustomerValidator : ICutomerValidator
    {
        public CustomerValidator()
        {
        }

        public void Validate(Customer customer, ICustomerRepository repository)
        {
            var context = new ValidationContext(customer, serviceProvider: null, items: null);
            var results = new List<ValidationResult>();

            if (!Validator.TryValidateObject(customer, context, results, validateAllProperties: true))
            {
                throw new CustoemrValidationException($"Invalid customer data: {string.Join(", ", results.Select(r => r.ErrorMessage))}");
            }

            if (repository.GetCustomers().Any(c => c.Id == customer.Id))
            {
                throw new CustoemrValidationException($"ID {customer.Id} has been used before.");
            }
        }
    }
}
