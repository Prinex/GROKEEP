using Android.Accounts;
using Grokeep.Models;

namespace Grokeep.Services;

public interface IUserService
{
    Task<User> RetrieveUser(string accountUsername);
    Task<bool> AppendUser(User userModel);
    Task<bool> UpdateUser(User userModel);
    Task<bool> RemoveUser(User userModel);
}

