namespace Grokeep.Services;

public class ProductService : IProductService
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

    public async Task<List<Product>> RetrieveProducts(int inventoryID)
    {
        await DBInit();
        var products = await connection.Table<Product>().Where(i => i.GroceryInventoryID == inventoryID).ToListAsync();
        return products;
    }

    public async Task<bool> AppendProduct(Product product)
    {
        await DBInit();
        var response = await connection.InsertAsync(product);
        return response > 0;
    }

    public async Task<bool> UpdateProduct(Product product)
    {
        await DBInit();
        var response = await connection.UpdateAsync(product);
        return response > 0;
    }

    public async Task<bool> DeleteProduct(Product product)
    {
        await DBInit();
        var response = await connection.DeleteAsync(product);
        return response > 0;
    }
}

