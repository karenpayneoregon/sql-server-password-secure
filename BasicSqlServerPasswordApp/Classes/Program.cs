using System.Runtime.CompilerServices;
using BasicSqlServerPasswordApp.Models;

// ReSharper disable once CheckNamespace
namespace BasicSqlServerPasswordApp
{
    internal partial class Program
    {
        [ModuleInitializer]
        public static void Init()
        {
            WindowUtility.SetConsoleWindowPosition(WindowUtility.AnchorWindow.Center);
        }

        public static SelectionPrompt<MenuItem> SelectionPrompt()
        {
            SelectionPrompt<MenuItem> menu = new()
            {
                HighlightStyle = new Style(Color.Black, Color.White, Decoration.None)
            };

            menu.Title("[cyan]Select[/]");
            menu.AddChoices(new List<MenuItem>()
            {
                new () {Id = 1, Text = "Login", Action = LoginMain },
                new () {Id = 2, Text = "Add new user", Action = AddNewUser },
                new () {Id = -1,Text = "Exit"},
            });

            return menu;

        }
    }
}
