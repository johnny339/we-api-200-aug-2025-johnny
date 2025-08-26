using Marten;
using Microsoft.AspNetCore.Mvc;
using Software.Api.CatalogItems.Endpoints;
using Software.Api.CatalogItems.Entities;
using Software.Api.CatalogItems.Services;
using Software.Api.Vendors;

namespace Software.Api.CatalogItems;




public static class Extensions
{

    public static IServiceCollection AddCatalogItems(this IServiceCollection services)
    {
        services.AddScoped<ICheckForVendors, MartenVendorData>();
        return services;
    }
    public static WebApplication MapCatalogItems(this WebApplication builder)
    {
        builder.MapGet("/catalog-items", async ([FromServices] IDocumentSession session) => await session.Query<CatalogItemEntity>().ToListAsync()).RequireAuthorization();
        
        var group = builder.MapGroup("/vendors").RequireAuthorization(); // unless you are identified with a JWT

        group.MapGet("/{vendorId:guid}/catalog-items/{id:guid}", GetCatalogItem.Handle);
    
        group.MapGet("/{vendorId:guid}/catalog-items/", GetCatalogItems.Handle);

        group.MapPost("/{id:guid}/catalog-items", AddCatalogItem.Handle).RequireAuthorization("SoftwareCenter"); // and you are SoftwareCenter
       
        return builder;
    }
}