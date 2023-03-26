using System.Diagnostics;

namespace sebtaskAPI.Services
{
  public class ScheduledTask : IHostedService
  {
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<ScheduledTask> _logger;
    private readonly IConfiguration _configuration;

    private Timer _timer;
    public ScheduledTask( IServiceProvider serviceProvider, ILogger<ScheduledTask> logger, IConfiguration configuration)
    {
      _serviceProvider = serviceProvider;
      _logger = logger;
      _configuration = configuration;
    }


    public  Task StartAsync(CancellationToken cancellationToken)
    {
     
      var startTime = _configuration.GetValue<TimeSpan>("TaskSchedule:StartTime");
      var interval = _configuration.GetValue<TimeSpan>("TaskSchedule:Interval");
      var delay = startTime - DateTime.Now.TimeOfDay;
      if (delay < TimeSpan.Zero)
      {
        delay = TimeSpan.FromDays(1) + delay;
      }
      _timer = new Timer(async _ => await OnTimerFiredAsync(cancellationToken),
            null, (int)delay.TotalMilliseconds, (int)interval.TotalMilliseconds);
      return Task.CompletedTask;
    }

    public  Task StopAsync(CancellationToken cancellationToken)
    {
     _timer?.Dispose();
      return Task.CompletedTask;
    }

    private async Task OnTimerFiredAsync(CancellationToken cancellationToken)
    {
      try
      {
        using (var scope = _serviceProvider.CreateScope()) {
            var ecbService = scope.ServiceProvider.GetRequiredService<EcbService>();
            await ecbService.GetLiveCurrencyRates();
        }
        _logger.LogInformation($"Task executed at {DateTime.Now}");
      }
      catch(Exception ex) {
          throw new Exception(ex.Message);
      }
      
    }
  }
}
