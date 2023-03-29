namespace Grokeep.Services;
public class GroceryHistoryService : IGroceryHistoryService
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

    public async Task<List<GroceryHistory>> RetrieveHistory(int accountID)
    {
        await DBInit();
        var groceryHistory = await connection.Table<GroceryHistory>().Where(u => u.AccountID == accountID).ToListAsync();
        return groceryHistory;
    }

    public async Task<bool> AppendHistory(GroceryHistory history)
    {
        await DBInit();
        var request = await connection.InsertAsync(history);
        return request > 0;
    }

    public async Task<bool> RemoveHistory(GroceryHistory history)
    {
        await DBInit();
        var request = await connection.DeleteAsync(history);
        return request > 0;
    }

    public async Task<bool> UpdateHistory(string previousProduct, Product currentProduct)
    {
        await DBInit();

        var products = await RetrieveHistory(App.UserSessionData.AccountID);
        GroceryHistory productToUpdate = new GroceryHistory();
        for (int i = 0; i < products.Count; i++)
        {
            if (products[i].Description == previousProduct)
            {
                productToUpdate = products[i];
                break;
            }
        }

        productToUpdate.Description = currentProduct.Description;
        productToUpdate.Cost = currentProduct.Cost;
        productToUpdate.Location = currentProduct.Location;
        productToUpdate.DateBought = currentProduct.DateBought;
        
        var request = await connection.UpdateAsync(productToUpdate);
        return request > 0; 
    }
}

