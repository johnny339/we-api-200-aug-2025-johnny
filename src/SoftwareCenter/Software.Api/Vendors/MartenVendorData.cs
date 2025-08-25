
using Marten;
using Software.Api.CatalogItems;
using Software.Api.CatalogItems.Services;

namespace Software.Api.Vendors;

public class MartenVendorData(IDocumentSession session, HttpContext context ) : ICreateVendors, ILookupVendors, ICheckForVendors
{
    public async Task<VendorDetailsModel> CreateVendorAsync(VendorCreateModel request)
    {
        // create the thing to save in the database, save it(?) return a VendorDetailsModel
        // create insert statement, run it the database.
        /// Mapping - Getting from Point A -> Point B
     
        var name = context.User?.Identity?.Name ?? "";
        var vendorToSave = request.MapToEntity(Guid.NewGuid(), name);

        session.Store(vendorToSave);
        await session.SaveChangesAsync();
        var response = new VendorDetailsModel(vendorToSave.Id, vendorToSave.Name, vendorToSave.Url, vendorToSave.Contact, vendorToSave.CreatedBy, vendorToSave.CreatedOn);
        return response;
    }

    public async Task<bool> DoesVendorExistAsync(Guid id, CancellationToken token)
    {
        return await session.Query<VendorEntity>().AnyAsync(v => v.Id == id, token);
    }

    public async Task<IReadOnlyList<VendorSummaryItem>> GetAllVendorsAsync(CancellationToken token)
    {
        var results = await session.Query<VendorEntity>()
          .OrderBy(r => r.CreatedOn)
          .Select(r => new VendorSummaryItem { Id = r.Id, Name = r.Name, })
            .ToListAsync();

        // Select -> Map
       //var response = results.Select(r => new VendorSummaryItem { Id = r.Id, Name = r.Name, }).ToList();
        return results;
    }

    public async Task<VendorDetailsModel?> GetVendorByIdAsync(Guid id, CancellationToken token)
    {
        var entity = await session.Query<VendorEntity>().Where(v => v.Id == id).SingleOrDefaultAsync(token);
        if (entity == null)
        {
            return null;
        } else
        {
            return entity.MapToDetails(); 
        }
    }
}


public class VendorEntity
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Url { get; set; } = string.Empty;
    public PointOfContact? Contact { get; set; }
    public string CreatedBy { get; set; } = string.Empty;
    public DateTimeOffset CreatedOn { get; set; }

    public VendorDetailsModel MapToDetails()
    {
        return new VendorDetailsModel(Id, Name, Url, Contact!, CreatedBy, CreatedOn);
    }
}

public class SomeUtility
{
    private readonly HttpContext _context;

    public SomeUtility(IHttpContextAccessor accessor)
    {
        _context = accessor.HttpContext; /// CAN BLOW UP IN PRODUCTION WHEN YOU ACCESS IT.
    }
}