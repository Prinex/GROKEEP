namespace Grokeep.Models;

public class Product
{
    private bool doesExist = true;
    public bool DoesExist { get => doesExist; set => doesExist = value; }

    [Column("productID"), PrimaryKey, AutoIncrement, NotNull, Unique]
    public int ProductID { get; set; }

    [Column("groceryInventoryID"), ForeignKey(typeof(GroceryInventory)), NotNull]
    public int GroceryInventoryID { get; set; }

    [Column("title"), NotNull]
    public string Title { get; set; }

    [Column("price"), NotNull]
    public double Price { get; set; }

    [Column("store"), NotNull]
    public string Store { get; set; }

    [Column("dateBought"), NotNull]
    public string DateBought { get; set; }
}

