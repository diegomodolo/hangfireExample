using Hangfire;
using Hangfire.States;

namespace HangfireExample.Framework;

public abstract class BackgroundJobBase : IBackgroundJob
{
    private readonly ILogger _logService;

    protected BackgroundJobBase(ILogger logService)
    {
        _logService = logService;
    }

    #region Implementation of IBackgroundJob

    public Guid JobId { get; } = Guid.NewGuid();
    public abstract string JobName { get; }
    public abstract EnqueuedState Queue { get; }
    public string StartedBy { get; }
    public DateTime StartDate { get; }


    [JobDisplayName("{0}")]
    public async Task Execute(string jobName)
    {
        try
        {
            await OnExecuteAsync(jobName);
            _logService.LogInformation($"{jobName} finished");
        }
        catch (Exception e)
        {
            _logService.LogError($"{jobName} failed - {e.Message}");
            throw;
        }
    }

    protected abstract Task OnExecuteAsync(string jobName);

    #endregion
}