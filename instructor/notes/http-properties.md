# Http Properties

- This is sort of like a rubric for some basic HTTP code reivews.

How do you choose what method to use, and what do you have to ensure if you are using these methods



| Method | Resource Type | Idempotent | Safe | Cacheable | Semantics |
GET       Collection       True         True   True GET me a representation of this resource.
GET       Document         True         True   True GET me a representation of this resource.
POST      Collection       False        False  False* Consider adding to your collection.
POST      Document         False        False  False What does this even mean? Not really used, except in "emergencies"
PUT       Collection       True         False  False Replace the entire collection with this body I'm sending
PUT       Document         True         False  False Replace this document with this body
DELETE    Collection       True         False  False REmove this whole collection (from the API)
DELETE    Document         True         False  False Remove this document (from the API)


GET /employees/bob-smith

{
    "id": "78397937",
    "name": {
        "first": "Bob",
        "last": "Smith"
    }, 
    "contactMechanisms": {
        "Email": "bob@company.com",
        "Phone": "867-5309"
    }
}

PUT /employees/bob-smith/contact-mechanisms
{
"email": "bob@compuserve.com"

}

GET /employees/bob-smith/contact-mechanism


{
"Email": "bob@company.com",
"Phone": "867-5309"
}

## Definitions


- Resources Types: Collection, Document, "Store" (like our /employees), or a "Controller"
    - "Store" is a collection that "mirrors client state on the server", but mostly like our /employee thing
    - How would an authenticated client know that to submit a problem with software they should do this:
        - POST /employees/{id}/problems
            - that "bob-smith" thing HAS to be in the Authorization header (a JWT or whatever)
        - POST /employee/problems 
        - Another example:
            - GET /shopping-cart
    - Controller is when things are weird and don't map to HTTP methods directly. I don't do these hardly ever.
        - POST /customer/claims - Binary - Either happy or sad. 400-499 or 200-299
            - 201 Created
              Location: /customer/claims/39739793793
        - POST /customer/claims/39739793793/resubmit
        [HttpPost("/customer/claims/{id})]
        [HttpPost("/customer/claims/{id}/resubmit)]

- A "view" of a resource at a specific point in time in a specific format (media type)
- Idempotent - doing this multiple times is the same as doing it once.
- Safe - nothing should change about the domain from the POV of the api because of this operation
  - DON'T USE GET REQUESTS FOR ANYTHING OTHER THAN GETTING A REPRESENTATION
- Cacheable - RFC 2616 says responses SHOULD have cache headers. But not everything is cacheable.
        each GET response SHOULD HAVE a cache control header that says how this should be dealt with in terms of "freshness"



GET /shopping-cart



POST /vips


201 Created
Location /vips/38973978937
Cache-Control: max-age: 18

{
    ... the data I'd get from that location


}


GET /vips/38973978937

the .net HTTP client has NO local cache at all.


GET /vehicles

