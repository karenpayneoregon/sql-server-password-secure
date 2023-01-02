using System.Security;

namespace SqlServerUtilities.Models;

public class User
{
    public int Id { get; set; }
    public required string  Name { get; set; }
    public required SecureString Password { get; set; }
    public override string ToString() => $"{Id} {Name}";

}