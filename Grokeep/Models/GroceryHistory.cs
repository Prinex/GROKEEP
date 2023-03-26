namespace Grokeep.Models;

public class GroceryHistory
{
    [Column("historyID"), PrimaryKey, AutoIncrement, NotNull, Unique]
    public int HistoryID { get; set; }

    [Column("accountID"), ForeignKey(typeof(User)), NotNull]
    public int AccountID { get; set; }

    [Column("title"), NotNull]
    public string Title { get; set; }

    [Column("price"), NotNull]
    public double Price { get; set; }

    [Column("store"), NotNull]
    public string Store { get; set; }

    [Column("dateBought"), NotNull]
    public string DateBought { get; set; }
}

