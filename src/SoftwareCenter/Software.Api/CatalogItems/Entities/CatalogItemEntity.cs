namespace Software.Api.CatalogItems.Entities;

public class CatalogItemEntity
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Version { get; set; } = string.Empty;

    public Guid VendorId { get; set; }
}