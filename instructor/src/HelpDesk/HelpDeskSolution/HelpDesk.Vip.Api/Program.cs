using Marten;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddSingleton(_ => TimeProvider.System);
builder.Services.AddOpenApi();
builder.Services.AddMarten(opts =>
{
    var connectionString = builder.Configuration.GetConnectionString("vips") ??
                           throw new Exception("Missing connection string");
    opts.Connection(connectionString);
}).UseLightweightSessions();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}
app.MapControllers();
app.Run(); // <- here is when it is running! 