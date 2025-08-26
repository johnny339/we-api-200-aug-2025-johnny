namespace Software.Api.CatalogItems.Representations;

public record CatalogItemResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Version { get; set; } = string.Empty;

    public Guid VendorId { get; set; }
}