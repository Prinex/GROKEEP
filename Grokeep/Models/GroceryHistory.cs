namespace Grokeep.Models;

public class GroceryHistory
{
    [Column("historyID"), PrimaryKey, AutoIncrement, NotNull, Unique]
    public int HistoryID { get; set; }

    [Column("accountID"), ForeignKey(typeof(User)), NotNull]
    public int AccountID { get; set; }

    [Column("description"), NotNull]
    public string Description { get; set; }

    [Column("cost"), NotNull]
    public double Cost { get; set; }

    [Column("location"), NotNull]
    public string Location { get; set; }

    [Column("dateBought"), NotNull]
    public string DateBought { get; set; }
}

