using Marten;
using Microsoft.AspNetCore.Http.HttpResults;
using Software.Api.CatalogItems.Entities;
using Software.Api.CatalogItems.Representations;

namespace Software.Api.CatalogItems.Endpoints;

public static class GetCatalogItem
{
    public static async Task<Results<Ok<CatalogItemResponse>, NotFound<string>>> Handle(Guid id, Guid vendorId, IDocumentSession session, CancellationToken token)
    {
        var response = await session.Query<CatalogItemEntity>().Where(c => c.Id == id && c.VendorId == vendorId)
            .Select(c => new CatalogItemResponse
            {
                Id = c.Id,
                Name = c.Name,
                Description = c.Description,
                Version = c.Version,
                VendorId = c.VendorId,
            })
            .FirstOrDefaultAsync(token);    
        if (response != null)
        {
            return TypedResults.Ok(response);
        }
        else
        {
            return TypedResults.NotFound("That catalog item doesn't exist");
        }
    }
    
    
}