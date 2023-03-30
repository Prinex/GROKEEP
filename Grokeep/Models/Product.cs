namespace Grokeep.Models;

public class Product
{
    private bool doesExist = true;
    public bool DoesExist { get => doesExist; set => doesExist = value; }

    [Column("productID"), PrimaryKey, AutoIncrement, NotNull, Unique]
    public int ProductID { get; set; }

    [Column("groceryInventoryID"), ForeignKey(typeof(GroceryInventory)), NotNull]
    public int GroceryInventoryID { get; set; }

    [Column("description"), NotNull]
    public string Description { get; set; }

    [Column("cost"), NotNull]
    public double Cost { get; set; }

    [Column("Location"), NotNull]
    public string Location { get; set; }

    [Column("dateBought"), NotNull]
    public string DateBought { get; set; }
}

