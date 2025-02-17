using Hangfire;
using Jobs.Jobs;
using Microsoft.EntityFrameworkCore;
using Shared.Context;
using Shared.Interfaces;
using Shared.Repositories;

var builder = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {
        services.AddDbContext<HahnAppContext>(options =>
            options.UseSqlServer(hostContext.Configuration.GetConnectionString("DefaultConnection")));

        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

        services.AddHttpClient();

        services.AddScoped<UpsertDataJob>();

        services.AddHangfire(configuration =>
        {
            configuration.SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                         .UseSimpleAssemblyNameTypeSerializer()
                         .UseRecommendedSerializerSettings()
                         .UseSqlServerStorage(hostContext.Configuration.GetConnectionString("HangfireConnection"));
        });
        services.AddHangfireServer();
    });

var host = builder.Build();

using (var scope = host.Services.CreateScope())
{
    var recurringJobManager = scope.ServiceProvider.GetRequiredService<IRecurringJobManager>();

    recurringJobManager.AddOrUpdate<UpsertDataJob>(
        "upsert-dog-facts-job",
        job => job.Execute(),
        Cron.Hourly);
}

await host.RunAsync();
