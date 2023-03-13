using Android.App;
using Grokeep.Models;
using SQLite;
using SQLiteNetExtensions.Attributes;

namespace Grokeep.Services;

public class UserService : IUserService
{
    public SQLiteAsyncConnection connection;

    public const string dbName = "grokeep.db";
    public const SQLite.SQLiteOpenFlags flags = SQLite.SQLiteOpenFlags.ReadWrite | SQLite.SQLiteOpenFlags.Create | SQLiteOpenFlags.SharedCache;
    public string dbPath = Path.Combine(FileSystem.AppDataDirectory, dbName);
    
    public async Task DBInit()
    {
        
        if (connection == null) 
        {
            connection = new SQLiteAsyncConnection(dbPath, flags);
            await connection.CreateTableAsync<User>();
        }
    }
    public async Task<User> RetrieveUser(string accountUsername)
    {
        await DBInit();
        var request = await connection.Table<User>().Where(u => u.Username == accountUsername).FirstOrDefaultAsync();
        return request ?? new User() { DoesExist = false };
    }

    public async Task<bool> AppendUser(User userModel)
    {
        await DBInit();
        var request = await connection.InsertAsync(userModel);
        return request > 0;
    }

    public async Task<bool> RemoveUser(User userModel)
    {
        await DBInit();
        var request = await connection.DeleteAsync(userModel);
        return request > 0;
    }

    public async Task<bool> UpdateUser(User userModel)
    {
        await DBInit();
        var request = await connection.UpdateAsync(userModel);
        return request > 0;
    }
}

