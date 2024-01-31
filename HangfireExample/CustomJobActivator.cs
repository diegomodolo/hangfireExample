using Hangfire;

namespace HangfireExample;

public class CustomJobActivator : JobActivator
{
    private readonly IServiceProvider _serviceProvider;

    public CustomJobActivator(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public override object ActivateJob(Type jobType)
    {
        return _serviceProvider.GetService(jobType);
    }
}