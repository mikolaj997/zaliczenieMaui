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
            var tableCommand = @"
                CREATE TABLE IF NOT EXISTS Projects (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT, 
                    Name TEXT, 
                    Description TEXT,
                    Status TEXT DEFAULT 'Nierozpoczęty',
                    DueDate TEXT
                )";
            var createTable = new SqliteCommand(tableCommand, db);
            createTable.ExecuteNonQuery();
        }
    }


    public async Task AddProjectAsync(string name, string description, DateTime dueDate)
    {
        using (var db = new SqliteConnection($"Filename={_dbPath}"))
        {
            db.Open();
            var command = "INSERT INTO Projects (Name, Description, Status, DueDate) VALUES (@Name, @Description, @Status, @DueDate)";
            var insertCommand = new SqliteCommand(command, db);
            insertCommand.Parameters.AddWithValue("@Name", name);
            insertCommand.Parameters.AddWithValue("@Description", description);
            insertCommand.Parameters.AddWithValue("@Status", "Nierozpoczęty");
            insertCommand.Parameters.AddWithValue("@DueDate", dueDate.ToString("yyyy-MM-dd"));
            await insertCommand.ExecuteNonQueryAsync();
        }
    }

    public async Task<List<Project>> GetProjectsAsync()
    {
        var projects = new List<Project>();

        using (var db = new SqliteConnection($"Filename={_dbPath}"))
        {
            db.Open();
            var command = "SELECT Id, Name, Description, Status, DueDate FROM Projects";
            var selectCommand = new SqliteCommand(command, db);
            using (var reader = await selectCommand.ExecuteReaderAsync())
            {
                while (reader.Read())
                {
                    var project = new Project
                    {
                        Id = reader.GetInt32(0),
                        Name = reader.GetString(1),
                        Description = reader.GetString(2),
                        Status = reader.GetString(3),
                        DueDate = DateTime.Parse(reader.GetString(4))
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

    public async Task UpdateProjectStatusAsync(int projectId, string status)
    {
        using (var db = new SqliteConnection($"Filename={_dbPath}"))
        {
            db.Open();
            var command = "UPDATE Projects SET Status = @Status WHERE Id = @Id";
            var updateCommand = new SqliteCommand(command, db);
            updateCommand.Parameters.AddWithValue("@Status", status);
            updateCommand.Parameters.AddWithValue("@Id", projectId);
            await updateCommand.ExecuteNonQueryAsync();
        }
    }
}
