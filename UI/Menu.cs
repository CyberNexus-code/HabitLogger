using HabitLogger.Services;
using Spectre.Console;

namespace HabitLogger.UI
{
    internal class Menu
    {
        public bool RenderMenu(HabitUI habitUI)
        {
            var choice = AnsiConsole.Prompt(
            new SelectionPrompt<MenuOptions>()
                .Title("[bold yellow]Welcome to the Habit Logger!\nWhat would you like to do?[/]")
                .AddChoices(Enum.GetValues<MenuOptions>()));

            SelectionHandler selectionHandler = new SelectionHandler();

            return selectionHandler.HandleSelection(choice, habitUI);
        }
    }

    enum MenuOptions
    {
        AddHabit,
        CreateHabitEntry,
        ViewHabits,
        ViewHabitLogs,
        DeleteHabit,
        Exit
    }
}
