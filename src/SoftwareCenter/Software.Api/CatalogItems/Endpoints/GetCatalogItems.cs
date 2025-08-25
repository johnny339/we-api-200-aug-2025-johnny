using Microsoft.AspNetCore.Http.HttpResults;
using Software.Api.CatalogItems.Representations;
using Software.Api.Vendors;

namespace Software.Api.CatalogItems.Endpoints;

public static class GetCatalogItems
{
    public static async Task<Results<Ok<CatalogItemResponse>, NotFound>> Handle(
        Guid vendorId,
        CancellationToken token)
    {
        if(true) {
        var fakeCatalogItem = new CatalogItemResponse();
            return TypedResults.Ok(fakeCatalogItem);
        }
        else
        {
            return TypedResults.NotFound();
        }
    }
}