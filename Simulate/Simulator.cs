namespace Simulate
{
    public class Simulator : BackgroundService
    {
        private readonly ILogger<Simulator> _logger;
        static int customerId = 1;
        const string baseUri = "http://localhost:5000/customers";

        public Simulator(ILogger<Simulator> logger)
        {
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await SimulatePostRequests();
        }

        private async Task SimulatePostRequests()
        {
            int numberOfRequests = 5;
            int pauseTime = 5000;
            while (true)
            {
                using (var httpClient = new HttpClient())
                {
                    List<Task> tasks = new List<Task>();

                    for (int i = 0; i < numberOfRequests; i++)
                    {
                        var newCustomers = GenerateCustomers();
                        Task postTask = SendPostRequest(httpClient, baseUri, newCustomers);
                        tasks.Add(postTask);
                    }

                    for (int i = 0; i < numberOfRequests; i++)
                    {
                        Task getTask = SendGetRequest(httpClient, baseUri);
                        tasks.Add(getTask);
                    }

                    await Task.WhenAll(tasks);
                }

                Thread.Sleep(pauseTime);
            }
        }

        private List<Customer> GenerateCustomers()
        {
            var customerData = new List<Customer>();
            for (var i = 0; i < 2; i++)
            {
                var random = new Random();
                var firstName = GetRandomFirstName();
                var lastName = GetRandomLastName();
                var age = random.Next(10, 90);

                var customer = new Customer
                {
                    FirstName = firstName,
                    LastName = lastName,
                    Age = age,
                    Id = customerId++
                };

                customerData.Add(customer);
            }
            return customerData;
        }

        private async Task SendPostRequest(HttpClient httpClient, string uri, List<Customer> customers)
        {
            string jsonData = System.Text.Json.JsonSerializer.Serialize(customers);
            HttpContent content = new StringContent(jsonData, System.Text.Encoding.UTF8, "application/json");
            HttpResponseMessage response = await httpClient.PostAsync(uri, content);

            if (response.IsSuccessStatusCode)
                _logger.LogDebug($"POST Response: OK");
            else
            {
                string responseContent = await response.Content.ReadAsStringAsync();
                _logger.LogDebug($"POST Response: {responseContent}");
            }

        }

        private async Task SendGetRequest(HttpClient httpClient, string uri)
        {
            HttpResponseMessage response = await httpClient.GetAsync(uri);

            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();
                //_logger.LogDebug($"GET Response: {content}");
                _logger.LogDebug($"GET Response: OK");
            }
            else
            {
                _logger.LogDebug($"GET Response: {response.StatusCode}");
            }
        }

        private string GetRandomFirstName()
        {
            var firstNames = new List<string> { "Leia", "Sadie", "Jose", "Sara", "Frank", "Dewey", "Tomas", "Joel", "Lukas", "Carlos" };
            return firstNames[new Random().Next(firstNames.Count)];
        }

        private string GetRandomLastName()
        {
            var lastNames = new List<string> { "Liberty", "Ray", "Harrison", "Ronan", "Drew", "Powell", "Larsen", "Chan", "Anderson", "Lane" };
            return lastNames[new Random().Next(lastNames.Count)];
        }
    }
}
