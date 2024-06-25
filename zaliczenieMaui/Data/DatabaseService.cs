using Microsoft.Data.Sqlite;
using System.IO;
using System.Threading.Tasks;
using zaliczenieMaui.Models;

public class DatabaseService
{
    private readonly string _dbPath;

    public DatabaseService()
    {
        _dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "projectmanagement.db");
        InitializeDatabase();
    }

    private void InitializeDatabase()
    {
        using (var db = new SqliteConnection($"Filename={_dbPath}"))
        {
            db.Open();
            var tableCommand = "CREATE TABLE IF NOT EXISTS Projects (Id INTEGER PRIMARY KEY AUTOINCREMENT, Name TEXT, Description TEXT)";
            var createTable = new SqliteCommand(tableCommand, db);
            createTable.ExecuteNonQuery();
        }
    }

    public async Task AddProjectAsync(string name, string description)
    {
        using (var db = new SqliteConnection($"Filename={_dbPath}"))
        {
            db.Open();
            var command = "INSERT INTO Projects (Name, Description) VALUES (@Name, @Description)";
            var insertCommand = new SqliteCommand(command, db);
            insertCommand.Parameters.AddWithValue("@Name", name);
            insertCommand.Parameters.AddWithValue("@Description", description);
            await insertCommand.ExecuteNonQueryAsync();
        }
    }
    public async Task<List<Project>> GetProjectsAsync()
    {
        var projects = new List<Project>();

        using (var db = new SqliteConnection($"Filename={_dbPath}"))
        {
            db.Open();

            var command = "SELECT Id, Name, Description FROM Projects";
            var queryCommand = new SqliteCommand(command, db);

            using (var reader = await queryCommand.ExecuteReaderAsync())
            {
                while (reader.Read())
                {
                    var project = new Project
                    {
                        Id = reader.GetInt32(0),
                        Name = reader.GetString(1),
                        Description = reader.GetString(2)
                    };
                    projects.Add(project);
                }
            }
        }
        return projects;
    }

    public async Task DeleteProjectAsync(int id)
    {
        using (var db = new SqliteConnection($"Filename={_dbPath}"))
        {
            db.Open();
            var command = "DELETE FROM Projects WHERE Id = @Id";
            var deleteCommand = new SqliteCommand(command, db);
            deleteCommand.Parameters.AddWithValue("@Id", id);
            await deleteCommand.ExecuteNonQueryAsync();
        }
    }

}
