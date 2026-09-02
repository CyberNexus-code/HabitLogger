using Spectre.Console;
using HabitLogger.Services;
using HabitLogger.Entities;

namespace HabitLogger.UI
{
    internal class HabitUI
    {
        private readonly HabitRepository _habitRepository;
        private List<Habit> _habits;
        private List<HabitLogEntry> _habitLogs;

        public HabitUI(HabitRepository repository)
        {
            _habitRepository = repository;
            RefreshHabits();
            RefreshHabitLogs();
        }

        public void AddHabit()
        {
            Console.Clear();
            // Implementation for adding a habit
            var name = AnsiConsole.Ask<string>("Please enter a new Habit name");
            var measurement = AnsiConsole.Ask<string>("How will this habits quantity be measured? (e.g. Glass... of water)");

            _habitRepository.AddHabbit(name, measurement);

            AnsiConsole.MarkupLine($"[Green] Habbit: {name} has been successfully added to the available habits[/]");
            RefreshHabits();
        }

        public void EditHabit()
        {
            var choice = AnsiConsole.Prompt(new SelectionPrompt<Habit>()
                .Title("Please select a Habit to edit")
                .UseConverter(habit => $"{habit.Name} - {habit.Measurement}")
                .AddChoices(_habits));

            var newName = AnsiConsole.Ask<string>("Please enter a new name or leave blank to keep the same name:");
            var newMeasurement = AnsiConsole.Ask<string>("Please enter a new measurement or leave blank to keep the same measurement:");

            _habitRepository.EditHabit(choice, newName, newMeasurement);
        }
        public void ViewHabits()
        {
            var table = new Table()
                .AddColumn("Id")
                .AddColumn("Name")
                .AddColumn("Measurement");

            int id = 1;
            _habits.ForEach(x =>
            {
                table.AddRow(id.ToString(), x.Name, x.Measurement);
                id++;
            });

            AnsiConsole.Write(table);
            AnsiConsole.MarkupLine("Press A to add a new habit. \nPress E to edit a habit \nor press B to go back to the main menue.");
            var choise = Console.ReadKey();

            
            switch (char.ToUpper(choise.KeyChar))
            {
                case 'A':
                    AddHabit();
                    break;
                case 'E':
                    EditHabit();
                    break;
                default:
                    break;
            }

        }

        public void CreateHabitLog()
        {
            var choice = AnsiConsole.Prompt(new SelectionPrompt<Habit>().Title("Please select a habit:")
                .UseConverter(habit => $"{habit.Name} - {habit.Measurement}")
                .AddChoices(_habits));

            var quantity = AnsiConsole.Prompt(
                new TextPrompt<int>($"Please enter the quantiy in [yellow]{choice.Measurement}:[/]")
                .ValidationErrorMessage("[red]Please enter a valid number[/]"));


            var time = AnsiConsole.Prompt(
                new TextPrompt<DateTime>("When did you complete the habit? [yellow](mm/dd/yyyy HH:mm)[/]")
                .ValidationErrorMessage("[red]Please enter a valid date and time.[/]"));

            _habitRepository.CreateHabitLogEntry(choice, quantity, time);
            RefreshHabitLogs();
        }


        public void ViewHabitLog()
        {
            Console.Clear();

            var table = new Table()
                .AddColumn("Id")
                .AddColumn("Habit")
                .AddColumn("Quantity")
                .AddColumn("Measurement")
                .AddColumn("TimeStamp");

            int id = 1;

            _habitLogs.ForEach(x =>
            {
                table.AddRow(id.ToString(), x.Habit.Name, x.Quantity.ToString(), x.Habit.Measurement, x.TimeEntry.ToString());
                id++;

            });

            AnsiConsole.Write(table);
            AnsiConsole.MarkupLine("Press any key to return to the menu...");
            Console.ReadKey();
        }
        public void DeleteHabit()
        {
            var choice = AnsiConsole.Prompt(new SelectionPrompt<Habit>()
                .Title("Please select a habit to delete...")
                .UseConverter(habit => $"{habit.Name} - {habit.Measurement}")
                .AddChoices(_habits));

            _habitRepository.DeleteHabit(choice.Name);
            Console.ReadKey();

            RefreshHabits();
        }

        public void RefreshHabits()
        {
            _habits = _habitRepository.GetHabits();
        }

        public void RefreshHabitLogs()
        {
            _habitLogs = _habitRepository.GetHabitLogEntries(_habits);
        }

    }
}
