
using Software.Api.Vendors;
using FluentValidation.TestHelper;


namespace Software.Tests.Vendors;

[Trait("Category", "UnitIntegration")]
public class VendorCreateValidationTests
{
    [Theory]
    [MemberData(nameof(ValidExamples))]
    public void ValidVendorCreateRequests(VendorCreateModel model, string failureMessage)
    {
        var validation = new VendorCreateModelValidator();
        var validations = validation.TestValidate(model);
        Assert.True(validations.IsValid, failureMessage);
    }

    [Theory]
    [MemberData(nameof(InvalidExamples))]
    public void InvalidVendorCreateRequsts(VendorCreateModel model, string failureMessage)
    {
        var validation = new VendorCreateModelValidator();
        var validations = validation.TestValidate(model);
        Assert.False(validations.IsValid, failureMessage);
    }

    const int vNameMinLength = 3;
    const int vNameMaxLength = 100;
    static PointOfContact validPoc = new() { Name = "John", Email = "joe@aol.com", Phone = "555-1212" };
    public static IEnumerable<object[]> ValidExamples() =>
    [
        [
            new VendorCreateModel {Name = new string('X', vNameMinLength), Contact = validPoc, Url= "https://dog.com"},
            $"The minimum length of vendor name should be {vNameMinLength}"
    ],
        [
            new VendorCreateModel {Name = new string('X', vNameMaxLength), Contact = validPoc, Url= "https://dog.com"},
              $"The maximum length of vendor name should be {vNameMaxLength}"
    ],
         [
            new VendorCreateModel { Name = new string('X', vNameMinLength), Url="https://dog.com", Contact = new PointOfContact { Name="Paul", Phone="555-1212" } },

            "Point of Contact must have a phone or email"
            ],
        [
            new VendorCreateModel { Name = new string('X', vNameMinLength), Url="https://dog.com", Contact = new PointOfContact { Name="Paul", Email="paul@aol.com" } },

            "Point of Contact must have a phone or email"
            ],
          [
            new VendorCreateModel { Name = new string('X', vNameMinLength), Url="https://dog.com", Contact = new PointOfContact { Name="Paul", Email="paul@aol.com", Phone="555-1212" } },

            "POC can have both a phone and email"
            ]
        ];

    public static IEnumerable<object[]> InvalidExamples() =>
   [
        [
            new VendorCreateModel {Name = "", Contact = validPoc, Url= "https://dog.com"},
         $"The Name Cannot Be Empty and should have a minimum length of  {vNameMinLength}"
        ],
        //new object[]
        [
            new VendorCreateModel { Name = new string('X', vNameMinLength -1), Contact= validPoc, Url = "https://dog.com"},
             $"The Name Cannot Be Empty and should have a minimum length of  {vNameMinLength}"
        ],
        [
            new VendorCreateModel { Name = new string('X', vNameMaxLength + 1), Contact= validPoc, Url = "https://dog.com"},
              $"The maximum length of vendor name should be {vNameMaxLength}"
        ],
        [
            new VendorCreateModel { Name = new string('X', vNameMinLength), Url="", Contact = validPoc },
            "The Url cannot be empty"
            ],
         [
            new VendorCreateModel { Name = new string('X', vNameMinLength), Url="", Contact = new PointOfContact { Name="Paul" } },

            "Point of Contact must have a phone or email"
            ]
        ];

}
