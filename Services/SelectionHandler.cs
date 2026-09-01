using HabitLogger.UI;

namespace HabitLogger.Services
{
    internal class SelectionHandler
    {
        public bool HandleSelection(MenuOptions choice, HabitUI habit)
        {
            switch (choice)
            {
                case MenuOptions.AddHabit:
                    habit.AddHabit();
                    break;
                case MenuOptions.CreateHabitEntry:
                    habit.CreateHabitLog();
                    break;
                case MenuOptions.ViewHabits:
                    habit.ViewHabits();
                    break;
                case MenuOptions.DeleteHabit:
                    habit.DeleteHabit();
                    break;
                case MenuOptions.Exit:
                    return true;
            }
            return false;
        }
    }
}
