using HelpDesk.Api.Employee.Issues;

namespace HelpDesk.Api.Users;

// Todo: Make this "real"
public class UserManager : IProvideUserInfo
{
    Task<string> IProvideUserInfo.GetUserSubAsync(CancellationToken token)
    {
        return Task.FromResult("Bob@aol.com");
    }
}
