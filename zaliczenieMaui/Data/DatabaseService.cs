using Microsoft.Data.Sqlite;
using System.IO;
using zaliczenieMaui.Models;
using System.Threading.Tasks;



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


    public async System.Threading.Tasks.Task AddProjectAsync(string name, string description, DateTime dueDate)
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
                        DueDate = reader.IsDBNull(4) ? (DateTime?)null : DateTime.Parse(reader.GetString(4))
                    };
                    projects.Add(project);
                }
            }
        }

        return projects;
    }

    public async System.Threading.Tasks.Task DeleteProjectAsync(int id)
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

    public async System.Threading.Tasks.Task UpdateProjectStatusAsync(int projectId, string status)
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

    public static async Task<List<TaskModel>> GetTasksAsync(int projectId)
    {
        var tasks = new List<TaskModel>();
        using (var db = new SqliteConnection($"Filename={_dbPath}"))
        {
            db.Open();
            var command = new SqliteCommand("SELECT Id, Title, Description, ProjectId FROM Tasks WHERE ProjectId = @ProjectId", db);
            command.Parameters.AddWithValue("@ProjectId", projectId);

            using (var reader = await command.ExecuteReaderAsync())
            {
                while (reader.Read())
                {
                    tasks.Add(new TaskModel
                    {
                        Id = reader.GetInt32(0),
                        Title = reader.GetString(1),
                        Description = reader.GetString(2),
                        ProjectId = reader.GetInt32(3)
                    });
                }
            }
        }
        return tasks;
    }
    public async System.Threading.Tasks.Task AddTaskAsync(TaskModel task)
    {
        using (var db = new SqliteConnection($"Filename={_dbPath}"))
        {
            db.Open();
            var command = ("INSERT INTO Tasks (Title, Description, ProjectId) VALUES (@Title, @Description, @ProjectId)");
            var insertCommand = new SqliteCommand(command, db)
            insertCommand.Parameters.AddWithValue("@Title", title);
            insertCommand.Parameters.AddWithValue("@Description", task.Description);
            insertCommand.Parameters.AddWithValue("@ProjectId", task.ProjectId);
            await command.ExecuteNonQueryAsync();
        }
    }

  
}
