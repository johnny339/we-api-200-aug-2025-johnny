

namespace Demos.Api.Home;

public class HomeController(ILogger<HomeController> logger) : ControllerBase
{


    [HttpGet("/")]
    public async Task<Ok<HomePageResponse>> GetHome(
        [FromServices] ICountHits hitCounter,
        [FromServices] ClassDataService classDataService,
        CancellationToken token)
    {

        logger.LogInformation("Got a request for the home thingy with GET");
        return TypedResults.Ok(new HomePageResponse(await hitCounter.GetHitCount(token)));
    }

    [HttpPost("/")]
    public async Task<Ok<HomePageResponse>> PostSomething(
        [FromServices] ICountHits hitCounter,
        CancellationToken token)
    {
        return TypedResults.Ok(new HomePageResponse(await hitCounter.GetHitCount(token)));
    }
}

public record HomePageResponse(int HitCount);
