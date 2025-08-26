
namespace HelpDesk.Api.Employee.Issues;

public interface IProvideUserInfo
{
    Task<string> GetUserSubAsync(CancellationToken token);
}