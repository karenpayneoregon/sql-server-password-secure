using System.Security;
using BasicSqlServerPasswordApp.Classes;

namespace BasicSqlServerPasswordApp.Models;

public class User
{
    public int Id { get; set; }
    public required string  Name { get; set; }
    public required SecureString Password { get; set; }
    public override string ToString() => $"{Id} {Name}";

}