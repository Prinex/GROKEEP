using SQLite;

namespace Grokeep.Models;

public class User
{
    private bool doesExist = true;
    
    public bool DoesExist { get => doesExist; set => doesExist = value; }
    
    [Column("accountID"), PrimaryKey, AutoIncrement, NotNull, Unique]
    public int AccountID { get; set; }
    
    [Column("username"), NotNull, Unique]
    public string Username { get; set; }
    
    [Column("password"), NotNull, Unique]
    public string Password { get; set; }
}

