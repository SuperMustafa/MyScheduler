using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Persistance.Repositories;
using Services;
using System.Runtime.InteropServices;

public class ScheduleExecutionService : BackgroundService
{
    private readonly ILogger<ScheduleExecutionService> _logger;
    private readonly IServiceScopeFactory _scopeFactory;

    public ScheduleExecutionService(
        ILogger<ScheduleExecutionService> logger,
        IServiceScopeFactory scopeFactory)
    {
        _logger = logger;
        _scopeFactory = scopeFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)    //service checks every 30 seconds
    {
        _logger.LogInformation(" Schedule Execution Service started.");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                using var scope = _scopeFactory.CreateScope();

                var scheduleRepository = scope.ServiceProvider.GetRequiredService<IScheduleRepsitory>();
                var executorService = scope.ServiceProvider.GetRequiredService<IExecutorService>();

                // Detect platform and use correct time zone ID
                string timeZoneId = RuntimeInformation.IsOSPlatform(OSPlatform.Windows)
                    ? "Egypt Standard Time"
                    : "Africa/Cairo";

                var egyptTimeZone = TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);
                var now = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, egyptTimeZone);

                _logger.LogInformation($" Current Egypt time: {now:yyyy-MM-dd HH:mm:ss}");


                var schedules = await scheduleRepository.GetAllAsync();
                _logger.LogInformation($" Loaded {schedules.Count()} schedules from database.");

                foreach (var schedule in schedules)
                {
                    //////////////////////////////////////////await executorService.ExecuteScheduleAsync(schedule);
                    if (!schedule.Active)
                    {
                        _logger.LogInformation($" Skipping inactive schedule: {schedule.Name}");
                        continue;
                    }

                    var scheduleTime = schedule.Time;
                    var scheduleDays = schedule.Days ?? new List<string>();
                    var currentDay = now.DayOfWeek.ToString();

                    _logger.LogInformation($" Checking schedule: {schedule.Name} for day {currentDay} at {scheduleTime.Hours:D2}:{scheduleTime.Minutes:D2}");

                    if (!scheduleDays.Contains(currentDay, StringComparer.OrdinalIgnoreCase))
                    {
                        _logger.LogInformation($" Schedule '{schedule.Name}' not valid for today ({currentDay})");
                        continue;
                    }

                    if (now.Hour == scheduleTime.Hours && now.Minute == scheduleTime.Minutes)
                    {
                        _logger.LogInformation($" Executing schedule: {schedule.Name}");
                        await executorService.ExecuteScheduleAsync(schedule);
                    }
                    else
                    {
                        _logger.LogInformation($" Not yet time for schedule '{schedule.Name}' (Now: {now:HH:mm}, Scheduled: {scheduleTime.Hours:D2}:{scheduleTime.Minutes:D2})");
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❗ An error occurred in ScheduleExecutionService loop.");
            }

            await Task.Delay(TimeSpan.FromSeconds(30), stoppingToken);
        }

        _logger.LogInformation(" Schedule Execution Service stopping.");
    }
}
