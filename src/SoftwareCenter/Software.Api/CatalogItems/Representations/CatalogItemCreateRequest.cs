namespace Software.Api.CatalogItems.Representations;

public record CatalogItemCreateRequest
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Version { get; set; } = string.Empty;
}