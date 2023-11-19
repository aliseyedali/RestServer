using System.Text.Json;

namespace RESTServer
{
    public static class ApplicationBuilderExtensions
    {
        public static async Task<IApplicationBuilder> PrepareData(this IApplicationBuilder app)
        {
            using var scopedServices = app.ApplicationServices.CreateScope();

            var serviceProvider = scopedServices.ServiceProvider;
            var customerService = serviceProvider.GetService<ICustomerService>();

            var data = "[{ \"lastName\": \"Aaaa\", \"firstName\": \"Aaaa\", \"age\": 20, \"id\": 3 }" +
                ",{ \"lastName\": \"Aaaa\", \"firstName\": \"Bbbb\", \"age\": 56, \"id\": 2 }" +
                ",{ \"lastName\": \"Cccc\", \"firstName\": \"Aaaa\", \"age\": 32, \"id\": 5 }" +
                ",{ \"lastName\": \"Cccc\", \"firstName\": \"Bbbb\", \"age\": 50, \"id\": 1 }" +
                ",{ \"lastName\": \"Dddd\", \"firstName\": \"Aaaa\", \"age\": 70, \"id\": 4 }]";
            if (customerService.GetCustomers().Count == 0)
                customerService.AddCustomers(System.Text.Json.JsonSerializer.Deserialize<List<Customer>>(data, 
                    new System.Text.Json.JsonSerializerOptions { 
                        PropertyNamingPolicy = JsonNamingPolicy.CamelCase 
                    }));

            return app;
        }
    }
}
