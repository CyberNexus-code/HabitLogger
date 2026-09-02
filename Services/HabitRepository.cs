using HabitLogger.Entities;
using Microsoft.Data.Sqlite;
using Spectre.Console;

namespace HabitLogger.Services
{
    internal class HabitRepository
    {
        private SqliteConnection _connection;

        public HabitRepository(SqliteConnection connection)
        {
            _connection = connection;
        }

        public void AddHabbit(string name, string measurement)
        {
            try
            {
                _connection.Open();


                var query = @"INSERT OR IGNORE INTO Habits (Name, Measurement) "
                            + $"VALUES(@name, @measurement)";

                var command = new SqliteCommand(query, _connection);
                command.Parameters.AddWithValue("@name", name);
                command.Parameters.AddWithValue("@measurement", measurement);
                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error inserting habbbit into Habits table: " + ex.Message);
            }
            finally
            {
                _connection.Close();
            }
        }

        public List<Habit> GetHabits()
        {
            List<Habit> habitList = new();

            try
            {
                _connection.Open();
                var query = @"SELECT * FROM Habits";
                var command = new SqliteCommand(query, _connection);

                var read = command.ExecuteReader();

                while (read.Read())
                {
                    Habit habit = new Habit(Int32.Parse(read.GetString(0)), read.GetString(1), read.GetString(2));
                    habitList.Add(habit);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Trouble getting Habits: " + ex.Message);
            }
            finally
            {
                _connection.Close();
            }

            return habitList;
        }

        public void DeleteHabit(string name)
        {
            try
            {
                _connection.Open();

                var query = @"DELETE FROM Habits where Name = @name";
                var command = new SqliteCommand(query, _connection);

                command.Parameters.AddWithValue("@name", name);
                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting habit '{name}' from Habits table:" + ex.Message + "\nPress any key to continue...");
            }
            finally
            {
                _connection.Close();
            }
        }

        public void EditHabit(Habit habit, string newName, string newMeasurement)
        {
            var selectedhabit = habit.Name;

            habit.Updatename(newName);
            habit.UpdateMeasurement(newMeasurement);

            try
            {
                _connection.Open();

                var query = @"UPDATE Habits set Name = @NewName, Measurement = @measurement WHERE name = @selectedHabit";
                var command = new SqliteCommand(query, _connection);

                command.Parameters.AddWithValue("@NewName", habit.Name);
                command.Parameters.AddWithValue("@measurement", habit.Measurement);
                command.Parameters.AddWithValue("@selectedHabit", selectedhabit);

                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                AnsiConsole.MarkupLine("There was an error updating the habit: " + ex.Message + "\nPress any key to return to the menu...");
                Console.ReadKey();
            }
            finally
            {
                _connection.Close();
            }
        }

        public void CreateHabitLogEntry(Habit habit, int quantity, DateTime timeStamp)
        {
            try
            {
                _connection.Open();

                var query = @"INSERT OR IGNORE INTO HabitLogEntry (Habit, Quantity, TimeStamp) "
                            + "VALUES (@Habit, @Quantity, @TimeStamp)";

                var command = new SqliteCommand(query, _connection);
                command.Parameters.AddWithValue("@Habit", habit.Id);
                command.Parameters.AddWithValue("@Quantity", quantity);
                command.Parameters.AddWithValue("@TimeStamp", timeStamp);
                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception("Problem creating new habit Log Entry: " + ex.Message);
            }
            finally
            {
                _connection.Close();
            }
        }

        public List<HabitLogEntry> GetHabitLogEntries(List<Habit> habits)
        {
            try
            {

                List<HabitLogEntry> logEntries = new();

                _connection.Open();

                var query = @"SELECT * FROM HabitLogEntry ORDER BY Timestamp DESC";

                var command = new SqliteCommand(query, _connection);

                var read = command.ExecuteReader();

                while (read.Read())
                {
                    var habit = habits.Where(x => x.Id.ToString() == read.GetString(0)).First();
                    var habitLog = new HabitLogEntry(habit, Int32.Parse(read.GetString(2)), DateTime.Parse(read.GetString(3)));
                    logEntries.Add(habitLog);
                }

                return logEntries;
            }
            catch (Exception ex)
            {
                throw new Exception("Error getting habbit log entries" + ex.Message);
            }
            finally
            {
                _connection.Close();
            }
        }
    }
}
