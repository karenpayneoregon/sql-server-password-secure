using BasicSqlServerPasswordApp.Classes;
using SqlServerUtilities.Classes;
using SqlServerUtilities.Models;

namespace BasicSqlServerPasswordApp;

internal partial class Program
{
    static void Main(string[] args)
    {

        while (true)
        {
            Console.Clear();
            SpectreOperations.DrawMainHeader();
            var menuItem = AnsiConsole.Prompt(SelectionPrompt());
            if (menuItem.Id != -1)
            {
                menuItem.Action();
            }
            else
            {
                return;
            }
        }
    }


    /// <summary>
    /// Add new user using HASHBYTES
    /// </summary>
    private static void AddNewUser()
    {
        Console.Clear();

        SpectreOperations.DrawAddUserHeader();

        var name = SpectreOperations.AskLoginName();
        var password = SpectreOperations.AskPassword();

        var result = DataOperations.AddUser(new User() { Name = name, Password = password.ToSecureString() });

        AnsiConsole.MarkupLine($"[white]Id[/] {result} [white]for[/] {name}");
        AnsiConsole.MarkupLine("[white on blue]To menu[/]");
        Console.ReadLine();
    }

    /// <summary>
    /// Used to demo logging in a user
    /// </summary>
    private static void LoginMain()
    {
        Console.Clear();

        SpectreOperations.DrawLoginHeader();

        var name = SpectreOperations.AskLoginName();
        var password = SpectreOperations.AskPassword();

        Console.Clear();

        if (Sample3(name, password))
        {
            SpectreOperations.DrawWelcomeHeader();
        }
        else
        {
            SpectreOperations.DrawGoAwayHeader();
        }

        AnsiConsole.MarkupLine("[white on blue]To menu[/]");
        Console.ReadLine();
    }

    // old, may not be supported in newer versions of SQL-Server
    private static bool Sample1(string userName, string password)
        => DataOperations.ValidateUser(userName, password!.ToSecureString()!);
    
    // does combined name and password in one statement
    private static bool Sample3(string userName, string password) 
        => DataOperations.ValidateUser1(userName, password!.ToSecureString()!);

    // Same as Sample3 but returns the primary key
    private static (bool success, int? id) Sample4(string userName, string password) 
        => DataOperations.ValidateUser2(userName, password!.ToSecureString()!);

}