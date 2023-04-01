namespace Grokeep.Services;

public interface IGroceryHistoryService
{
    Task<List<GroceryHistory>> RetrieveHistory(int accountID);
    Task<bool> AppendHistory(GroceryHistory history);
    Task<bool> RemoveHistory(GroceryHistory history);
    Task<bool> UpdateHistory(string previousProduct, Product currentProduct);
    Task<bool> RemoveAllHistory(int accountID);
}

