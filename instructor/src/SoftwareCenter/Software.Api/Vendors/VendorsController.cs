using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Software.Api.Vendors;


[Authorize]
[ApiController] // make me come back to this
public class VendorsController : ControllerBase
{
    [HttpGet("/vendors")]
    public async Task<ActionResult> GetAllVendors(
        [FromServices] ILookupVendors vendorLookup,
        CancellationToken token)
    {

        IReadOnlyList<VendorSummaryItem> vendors  = await vendorLookup.GetAllVendorsAsync(token);
        return Ok(vendors);
    }

    [Authorize(Policy = "SoftwareCenterManager")]
    [HttpPost("/vendors")] // POST to a collection resource. 
    public async Task<ActionResult<VendorDetailsModel>> AddAVendorAsync(
        [FromBody] VendorCreateModel request,
        [FromServices] ICreateVendors vendorCreator,
        [FromServices] IValidator<VendorCreateModel> validator
        )
    {
        var user = User;
        var validations = await validator.ValidateAsync(request);

        if (!validations.IsValid)
        {
            //return BadRequest(); // just send a 400
            return BadRequest(validations.Errors); // send a 400 with some error information in it.
            // we also have problems+json - I'll come back to that
        }


        VendorDetailsModel response = await vendorCreator.CreateVendorAsync(request);

        return Created($"/vendors/{response.Id}", response);
    }


    [HttpGet("/vendors/{id:guid}")]
    public async Task<ActionResult> GetVendorByIdAsync([FromRoute] Guid id,

        [FromServices] ILookupVendors vendorLookup,
        CancellationToken token )
    {

       VendorDetailsModel? response = await vendorLookup.GetVendorByIdAsync(id, token);
        return response switch
        {
            null => NotFound(),
            _ => Ok(response)
        };
    }


}



public record VendorSummaryItem
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
}

public record PointOfContact
{
   
    public string Name { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
    public string Phone { get; init; } = string.Empty;
}

public class PointOfContactValidator : AbstractValidator<PointOfContact>
{
    public PointOfContactValidator()
    {
        RuleFor(c => c.Name).NotEmpty();
        RuleFor(c => c.Email).NotEmpty().When(c => c.Phone == "");
        RuleFor(c => c.Phone).NotEmpty().When(c => c.Email == "");
    }
}

public record VendorCreateModel
{

    public required string Name { get; init; } = string.Empty;
    public required string Url { get; init; } = string.Empty;
    public required PointOfContact Contact { get; init; }
    public VendorEntity MapToEntity(Guid id, string createdBy)
    {
        return new VendorEntity
        {
            Id = id,
            Contact = Contact,
            Name = Name,
            CreatedBy = createdBy,
            CreatedOn = DateTimeOffset.UtcNow,
            Url = Url,


        };
    }
};

public class VendorCreateModelValidator : AbstractValidator<VendorCreateModel>
{
    public VendorCreateModelValidator()
    {
        RuleFor(v => v.Name).NotEmpty().MinimumLength(3).MaximumLength(100);
        RuleFor(v => v.Url).NotEmpty();
        RuleFor(v => v.Contact).NotNull().SetValidator(validator: new PointOfContactValidator()); // The warning was because I had Contact as a nullable reference type.
    }
}

public record VendorDetailsModel(Guid Id, string Name, string Url, PointOfContact Contact, string CreatedBy, DateTimeOffset CreatedOn);