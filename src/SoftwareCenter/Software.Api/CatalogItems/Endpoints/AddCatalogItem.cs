using Marten;
using Microsoft.AspNetCore.Http.HttpResults;
using Software.Api.CatalogItems.Entities;
using Software.Api.CatalogItems.Representations;
using Software.Api.CatalogItems.Services;

namespace Software.Api.CatalogItems.Endpoints;

public static class AddCatalogItem
{
    public static async Task<Results<Ok<CatalogItemResponse>, NotFound<string>>> Handle(
        CatalogItemCreateRequest request, Guid id, ICheckForVendors vendorChecker, IDocumentSession session,
        CancellationToken token)
    {
        // validate
        // validate the body of this request
        // and validate that the id parameter is for a an existing vendor
        if (await vendorChecker.DoesVendorExistAsync(id, token))
        {
            // If good, create a CatalogItemEntity, Store it.
            var entityToSave = new CatalogItemEntity
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                Description = request.Description,
                Version = request.Version,
                VendorId = id,
            };
            session.Store(entityToSave);
            await session.SaveChangesAsync();
            // Map That to a Response.
            var response = new CatalogItemResponse
            {
                Id = entityToSave.Id,
                Name = entityToSave.Name,
                Description = entityToSave.Description,
                Version = entityToSave.Version,
                VendorId = id,
            };
            return TypedResults.Ok(response);
        }
        else
        {
            return TypedResults.NotFound("That vendor doesn't exist");
        }
    }
}