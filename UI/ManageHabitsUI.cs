using HabitLogger.Entities;
using HabitLogger.Services;
using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Text;

namespace HabitLogger.UI
{
    internal class ManageHabitsUI
    {
        private readonly HabitRepository _habitRepository;
        private List<Habit> _habits;

        public ManageHabitsUI(HabitRepository habitRepository)
        {
            _habitRepository = habitRepository;
            RefreshHabits();
        }

        public void Show()
        {
            while (true)
            {
                Console.Clear();

                var choice = ViewHabits();

                switch (choice)
                {
                    case HabitAction.Add:
                        AddHabit();
                        break;
                    case HabitAction.Edit:
                        EditHabit();
                        break;
                    case HabitAction.Delete:
                        DeleteHabit();
                        break;
                    case HabitAction.Back:
                        Console.Clear();
                        return;
                }
            }
        }
        public void AddHabit()
        {
            Console.Clear();
            // Implementation for adding a habit
            var name = AnsiConsole.Ask<string>("Please enter a new Habit name");
            var measurement = AnsiConsole.Ask<string>("How will this habits quantity be measured? (e.g. Glass... of water)");

            if(_habitRepository.AddHabit(name, measurement) > 0)
            {
                AnsiConsole.MarkupLine($"[Green] Habit: {name} has been successfully added to the available habits[/]\nPress any key to continue...");
                Console.ReadKey();
                RefreshHabits();
            }
            else
            {
                AnsiConsole.MarkupLine($"[red]A habit with that name already exists.[/]\nPress any key to continue...");
                Console.ReadKey();
            }

        }

        public void EditHabit()
        {
            Console.Clear();

            var choice = AnsiConsole.Prompt(new SelectionPrompt<Habit>()
                .Title("Please select a Habit to edit")
                .UseConverter(habit => $"{habit.Name} - {habit.Measurement}")
                .AddChoices(_habits));

            var newName = AnsiConsole.Prompt(
                new TextPrompt<string>("Please enter a new name or leave blank to keep the same name:")
                .AllowEmpty());
            var newMeasurement = AnsiConsole.Prompt(
                new TextPrompt<string>("Please enter a new measurement or leave blank to keep the same measurement:")
                .AllowEmpty());

            _habitRepository.EditHabit(choice, newName, newMeasurement);
        }
        public HabitAction ViewHabits()
        {
            Console.Clear();

            var table = new Table()
                .AddColumn("Id")
                .AddColumn("Name")
                .AddColumn("Measurement");

            _habits.ForEach(x =>
            {
                table.AddRow(x.Id.ToString(), x.Name, x.Measurement);
            });

            AnsiConsole.Write(table);

            return AnsiConsole.Prompt(new SelectionPrompt<HabitAction>()
                .Title("Please select an action to perform:")
                .UseConverter(x => x switch
                {
                    HabitAction.Add => "Add a new Habit",
                    HabitAction.Edit => "Edit an existing Habit",
                    HabitAction.Delete => "Delete an existing Habit",
                    HabitAction.Back => "Return to the previous menu"
                })
                .AddChoices(Enum.GetValues<HabitAction>()));

        }
        public void DeleteHabit()
        {
            Console.Clear();

            var choice = AnsiConsole.Prompt(new SelectionPrompt<Habit>()
                .Title("Please select a habit to delete...")
                .UseConverter(habit => $"{habit.Name} - {habit.Measurement}")
                .AddChoices(_habits));

            var confirmation = AnsiConsole.Prompt(new SelectionPrompt<string>()
                .Title($"Are you sure you want to delete Habit: '{choice}'?")
                .AddChoices("Yes", "No"));

            if(confirmation == "Yes")
            {
                _habitRepository.DeleteHabit(choice.Name);
                AnsiConsole.MarkupLine($"[red]{choice} has been deleted.\nPress any key to continue...");
                Console.ReadKey();
            }

            RefreshHabits();
        }
        public void RefreshHabits()
        {
            _habits = _habitRepository.GetHabits();
        }
    }
    enum HabitAction
    {
        Add,
        Edit,
        Delete,
        Back
    }
}
