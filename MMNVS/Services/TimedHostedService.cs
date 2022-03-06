using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Hosting.Server.Features;

namespace MMNVS.Services
{
    public class TimedHostedService : IHostedService, IDisposable
    {
        private readonly ILogger<TimedHostedService> _logger;
        private readonly IConfiguration _configuration;
        private Timer? _timer;
        public string BaseUrl { get; set; }

        public TimedHostedService(ILogger<TimedHostedService> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        public Task StartAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Timed Hosted Service running.");
            BaseUrl = _configuration.GetValue<string>("UPSCheck:BaseUrl");
            int time = Int32.Parse(_configuration.GetValue<string>("UPSCheck:Time"));

            _timer = new Timer(DoWork, null, TimeSpan.Zero,
                TimeSpan.FromSeconds(time));


            return Task.CompletedTask;
        }

        private void DoWork(object state)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(BaseUrl);

            _logger.LogInformation("UPS Check: {time}", DateTimeOffset.Now);
            try
            {
                HttpResponseMessage response = client.GetAsync("api/ups").Result;
                _logger.LogInformation(response.Content.ReadAsStringAsync().Result);
            }
            catch
            {

            }
        }

        public Task StopAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Timed Hosted Service is stopping.");

            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}
