
namespace HelpDesk.Api.Employee.Issues;


// This is a SERVICE that owns ALL communication with the HelpDesk.Vip.Api 
public class CheckVipService(HttpClient client)
{
    public async Task<List<string>> GetCurrentVipsAsync(CancellationToken token)
    {
        var response = await client.GetAsync("/help-desk/vips");

        response.EnsureSuccessStatusCode(); // more in a minute 200-299 

        var vips = await response.Content.ReadFromJsonAsync<List<string>>();
        if (vips is null)
        {
            return new List<string>();
        }
        else
        {
            return vips;
        }

    }
}