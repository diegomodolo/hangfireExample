using Hangfire.States;
using HangfireExample.Framework;

namespace HangfireExample;

public class CustomJob : BackgroundJobBase
{
    public CustomJob(ILogger<CustomJob> logService) : base(logService)
    {
    }

    public override string JobName { get; }
    public override EnqueuedState Queue { get; }

    protected override Task OnExecuteAsync(string jobName)
    {
        Console.WriteLine("Hello world from a custom job: {0}", jobName);

        return Task.FromResult(true);
    }
}