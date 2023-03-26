namespace Grokeep.Services;
public interface IGroceryInventoryService
{
    Task<List<GroceryInventory>> RetrieveInventories(int accountID);
    Task<bool> AppendInventory(GroceryInventory inventory);
    Task<bool> UpdateInventory(GroceryInventory inventory);
    Task<bool> RemoveInventory(GroceryInventory inventory);
}

