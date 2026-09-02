using HabitLogger.Services;
using Spectre.Console;

namespace HabitLogger.UI
{
    internal class MainMenu
    {
        public bool RenderMenu(OptionsUI optionsUi, ManageLogEntriesUI manageLogEntriesUi)
        {
            Console.Clear();

            var choice = AnsiConsole.Prompt(
            new SelectionPrompt<MenuOptions>()
                .Title("[bold yellow]Welcome to the Habit Logger!\nWhat would you like to do?[/]")
                .AddChoices(Enum.GetValues<MenuOptions>()));

            switch (choice)
            {
                case MenuOptions.CreateHabitEntry:
                    manageLogEntriesUi.CreateHabitLog();
                    break;
                case MenuOptions.Options:
                    optionsUi.Show();
                    break;
                case MenuOptions.Exit:
                    return true;
            }

            return false;
        }
    }

    enum MenuOptions
    {
        CreateHabitEntry,
        Options,
        Exit
    }
}
