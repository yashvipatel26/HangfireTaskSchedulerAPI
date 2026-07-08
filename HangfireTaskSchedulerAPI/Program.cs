using Hangfire;
using Hangfire.SqlServer;
using HangfireTaskSchedulerAPI.Jobs;
using HangfireTaskSchedulerAPI.Services;
using HangfireTaskSchedulerAPI.Data;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("HangfireConnection")));

builder.Services.AddHangfire(configuration => configuration
    .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
    .UseSimpleAssemblyNameTypeSerializer()
    .UseRecommendedSerializerSettings()
    .UseSqlServerStorage(
        builder.Configuration.GetConnectionString("HangfireConnection")));

builder.Services.AddHangfireServer();
builder.Services.AddTransient<DailyTaskJob>();

builder.Services.AddTransient<EmailService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var recurringJobManager = scope.ServiceProvider.GetRequiredService<IRecurringJobManager>();

    // Daily
    recurringJobManager.AddOrUpdate<DailyTaskJob>(
        "DailyTaskJob",
        job => job.SendDailyTask(),
        "30 5 * * *");

    // Every Hour
    recurringJobManager.AddOrUpdate<DailyTaskJob>(
        "HourlyTaskJob",
        job => job.SendHourlyTask(),
        "0 * * * *");

    // Weekly
    recurringJobManager.AddOrUpdate<DailyTaskJob>(
        "WeeklyTaskJob",
        job => job.SendWeeklyTask(),
        "30 5 * * 1");

    // Monthly
    recurringJobManager.AddOrUpdate<DailyTaskJob>(
        "MonthlyTaskJob",
        job => job.SendMonthlyTask(),
        "30 5 1 * *");
}

app.UseHangfireDashboard();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
