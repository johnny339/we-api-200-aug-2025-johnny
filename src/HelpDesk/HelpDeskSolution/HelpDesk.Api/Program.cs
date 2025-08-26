

using System.Text.Json;
using System.Text.Json.Serialization;
using HelpDesk.Api;
using HelpDesk.Api.Employee.Issues;
using HelpDesk.Api.Users;
using Marten;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);
builder.AddOtel();

builder.Services.AddControllers().AddJsonOptions(
    options =>
    {
        // options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseUpper; // ImpactRadius -> IMPACT_RADIUS
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()); // No brainer.. Should be the default, imo. 
        //options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;  // a little weirder, but I like it.
    });
builder.Services.AddOpenApi();
var connectionString = builder.Configuration.GetConnectionString("issues") ?? throw new Exception("No Connection String");
builder.Services.AddMarten(options =>
{
    options.Connection(connectionString);
}).UseLightweightSessions();


builder.Services.AddSingleton<TimeProvider>(_ => TimeProvider.System);
builder.Services.AddScoped<IProvideUserInfo, UserManager>();
builder.Services.AddHostedService<VipNotificationBackgroundWorker>();
var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.MapControllers();
app.Run();

