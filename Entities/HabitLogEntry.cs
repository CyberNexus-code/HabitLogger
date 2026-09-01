using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Text;

namespace HabitLogger.Entities
{
    internal class HabitLogEntry
    {
        public Habit Habit { get; private set; }
        public int Quantity { get; private set; }
        public DateTime TimeEntry { get; private set; } = DateTime.UtcNow;

        public HabitLogEntry(Habit habit, int quantity, DateTime timeEntry)
        {
            Habit = habit;
            Quantity = ValidateQuantity(quantity);
            TimeEntry = ValidateTimeEntry(timeEntry);
        }

        private DateTime ValidateTimeEntry(DateTime timeEntry)
        {
            if (timeEntry > DateTime.UtcNow)
            {
                throw new ArgumentOutOfRangeException("The time entry cannot be in the future.");
            }

            return timeEntry;
        }

        private int ValidateQuantity(int quantity)
        {
            if (quantity <= 0)
            {
                throw new ArgumentException("Please enter a quantity greater than 0 for your Habit.");
            }

            return quantity;
        }
    }
}
