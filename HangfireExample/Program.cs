using Hangfire;
using HangfireExample;
using HangfireExample.Framework;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHangfire(configuration => configuration.SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
    .UseSimpleAssemblyNameTypeSerializer()
    .UseRecommendedSerializerSettings()
    .UseSqlServerStorage(builder.Configuration.GetConnectionString("HangfireConnection")));

builder.Services.AddLogging();

// GlobalConfiguration.Configuration.UseActivator(new CustomJobActivator(builder.Services.BuildServiceProvider()));

builder.Services.AddHangfireServer(options =>
{
    // options.Activator = new CustomJobActivator(builder.Services.BuildServiceProvider());
});

builder.Services.AddScoped<IBackgroundJob, CustomJob>();

var app = builder.Build();

app.UseStaticFiles();

app.UseHangfireDashboard();

var backgroundJobClient = app.Services.GetService<IBackgroundJobClient>();
backgroundJobClient.Enqueue(() => Console.WriteLine("Hello world from Hangfire!"));
backgroundJobClient.Enqueue<CustomJob>(x => x.Execute("19"));

RecurringJob.AddOrUpdate("say-hello", () => Console.WriteLine("Hello world from Hangfire!"), Cron.Minutely);

app.MapGet("/", () => "Hello World!");

app.Run();