using Hangfire.States;

namespace HangfireExample.Framework;

public interface IBackgroundJob
{
    Guid JobId { get; }
    string JobName { get; }
    EnqueuedState Queue { get; }
    DateTime StartDate { get; }
    string StartedBy { get; }
    Task Execute(string jobName);
}