


using Marten;
using Software.Api.CatalogItems;
using Software.Api.Vendors;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddAuthentication().AddJwtBearer();
builder.Services.AddControllers();

builder.Services.AddOpenApi();

builder.Services.AddAuthorizationBuilder().AddPolicy("SoftwareCenterManager", pol =>
{
    pol.RequireRole("SoftwareCenter");
    pol.RequireRole("Manager");
}).AddPolicy("SoftwareCenter", pol =>
{
    pol.RequireRole("SoftwareCenter");
});


// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var connectionString = builder.Configuration.GetConnectionString("software") ?? throw new Exception("No Connection String Found In Environment");

builder.Services.AddMarten(opts =>
{
    opts.Connection(connectionString);
}).UseLightweightSessions();

// an API, a "scoped" service means "use the same one for the entire request/response"
builder.Services.AddVendors();
builder.Services.AddCatalogItems();



var app = builder.Build(); // The line in the sand, above this is configuring services.
                           // Below this is configuring middleware.

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/openapi/v1.json", "v1");

    });
}

app.UseAuthentication();
app.UseAuthorization();
// Route Table:
// POST /vendors -> Create the Vendor Controllerr, and Call the AddVendorAsync Method.

app.MapControllers(); // before we run the application, go find all the "controllers" 

app.MapCatalogItems();


app.Run(); // an endless while loop, basically. it "blocks", keeps running here forever, waiting for requests.


public partial class Program;