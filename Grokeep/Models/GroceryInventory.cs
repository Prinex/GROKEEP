namespace Grokeep.Models;
public class GroceryInventory
{
    [Column("groceryInventoryID"), PrimaryKey, NotNull, Unique]
    public int GroceryInventoryID { get; set; }

    [Column("accountID"), ForeignKey(typeof(User)), NotNull]
    public int AccountID { get; set; }

    [Column("groceryListName"), NotNull]
    public string GroceryListName { get; set; }
}

