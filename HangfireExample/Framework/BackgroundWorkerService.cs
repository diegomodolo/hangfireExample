using Hangfire;
using Hangfire.States;
using Hangfire.Storage;

namespace HangfireExample.Framework;

public interface IBackgroundWorkerService
{
    void EnqueueJob(IBackgroundJob job, EnqueuedState state = null);
    void EnqueueJob<T>() where T : IBackgroundJob;
    void EnqueueJob<T>(EnqueuedState queue) where T : IBackgroundJob;
    IEnumerable<RecurringJobDto> GetRecurringJobs();
}

public class BackgroundWorkerService : IBackgroundWorkerService
{
    private readonly IBackgroundJobClient _backgroundJobClient;
    private readonly IServiceProvider _serviceProvider;

    public BackgroundWorkerService(IBackgroundJobClient backgroundJobClient, IServiceProvider serviceProvider)
    {
        _backgroundJobClient = backgroundJobClient;
        _serviceProvider = serviceProvider;
    }

    public void EnqueueJob(IBackgroundJob job, EnqueuedState state = null)
    {
        if (job.Queue != null)
        {
            _backgroundJobClient.Create(() => job.Execute(job.JobName), state ?? job.Queue);
            return;
        }

        _backgroundJobClient.Enqueue(() => job.Execute(job.JobName));
    }

    private IBackgroundJob GetJob<T>()
    {
        return (IBackgroundJob)_serviceProvider.GetService(typeof(T));
    }

    #region Implementation of IBackgroundWorkerService

    public void EnqueueJob<T>() where T : IBackgroundJob
    {
        // BackgroundJobBase jobBase = (BackgroundJobBase) Activator.CreateInstance(typeof(T));
        // EnqueueJob(jobBase);
        EnqueueJob(GetJob<T>());
    }

    public void EnqueueJob<T>(EnqueuedState queue) where T : IBackgroundJob
    {
        // BackgroundJobBase jobBase = (BackgroundJobBase) Activator.CreateInstance(typeof(T));
        // EnqueueJob(jobBase, queue);
        EnqueueJob(GetJob<T>(), queue);
    }

    public IEnumerable<RecurringJobDto> GetRecurringJobs()
    {
        return JobStorage.Current.GetConnection().GetRecurringJobs();
    }

    #endregion
}