
using HabitLogger.Interfaces;
using HabitLogger.Services;
using Microsoft.Data.Sqlite;
using HabitLogger.UI;

string ConnectionString  = "Data Source=./db/database.db";

Console.WriteLine("starting Program.cs");

IInitializeDB database = new InitializeDB();
database.InitializeDatabase();

using var connection = new SqliteConnection(ConnectionString);


Console.WriteLine("Checking Tables...");

var table = new InitializeTable(connection);

table.InitializeHabitTable();
table.SeedTable();


var repository = new HabitRepository(connection);
var HabitUI = new HabitUI(repository);

var menu = new Menu();
Console.Clear();

while (true)
{
    Console.Clear();

    if (menu.RenderMenu(HabitUI))
    {
        break;
    }
}








