namespace Software.Api.CatalogItems.Services;

public interface ICheckForVendors
{
    Task<bool> DoesVendorExistAsync(Guid id, CancellationToken token);
}