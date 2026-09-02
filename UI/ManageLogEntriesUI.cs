using HabitLogger.Entities;
using HabitLogger.Services;
using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Text;

namespace HabitLogger.UI
{
    internal class ManageLogEntriesUI
    {
        private readonly HabitRepository _habitRepository;
        private List<Habit> _habits;
        private List<HabitLogEntry> _habitLogs;

        public ManageLogEntriesUI(HabitRepository habitRepository)
        {
            _habitRepository = habitRepository;
            RefreshHabitLogs();
        }

        public void Show()
        {
            while (true)
            {
                Console.Clear();

                var choise = AnsiConsole.Prompt(new SelectionPrompt<ManageLogEntriesOptions>()
                    .Title("What do you want to do?")
                    .UseConverter(x => x switch
                    {
                        ManageLogEntriesOptions.CreateHabitLog => "Create a new Log Entry",
                        ManageLogEntriesOptions.ViewHabitLogs => "View your Habit logs",
                        ManageLogEntriesOptions.Back => "Back to previous menu..."
                    })
                    .AddChoices(Enum.GetValues<ManageLogEntriesOptions>()));

                switch (choise)
                {
                    case ManageLogEntriesOptions.CreateHabitLog:
                        CreateHabitLog();
                        break;
                    case ManageLogEntriesOptions.ViewHabitLogs:
                        ViewHabitLog();
                        break;
                    case ManageLogEntriesOptions.Back:
                        return;
                }
            }
        }
        public void CreateHabitLog()
        {
            Console.Clear();

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
            RefreshHabitLogs();
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

        public void RefreshHabitLogs()
        {
            _habits = _habitRepository.GetHabits();
            _habitLogs = _habitRepository.GetHabitLogEntries(_habits);
        }
    }

    enum ManageLogEntriesOptions
    {
        CreateHabitLog,
        ViewHabitLogs,
        Back
    }
}
