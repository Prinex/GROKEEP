namespace Grokeep.Services;

public class GroceryInventoryService : IGroceryInventoryService
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
            await connection.CreateTableAsync<GroceryInventory>();
            await connection.CreateTableAsync<GroceryHistory>();
            await connection.CreateTableAsync<Product>();
            // activate fk
            await connection.ExecuteAsync("PRAGMA foreign_keys=ON");
        }
    }

    public async Task<bool> AppendInventory(GroceryInventory inventory)
    {
        await DBInit();
        var request = await connection.InsertAsync(inventory);
        return request > 0;
    }

    public async Task<bool> RemoveInventory(GroceryInventory inventory)
    {
        await DBInit();
        var request = await connection.DeleteAsync(inventory);
        return request > 0;
    }

    public async Task<List<GroceryInventory>> RetrieveInventories(int accountID)
    {
        await DBInit();
        var inventories = await connection.Table<GroceryInventory>().Where(u => u.AccountID == accountID).ToListAsync();
        return inventories;
    }

    public async Task<bool> UpdateInventory(GroceryInventory inventory)
    {
        await DBInit();
        var request = await connection.UpdateAsync(inventory);
        return request > 0;
    }
}

