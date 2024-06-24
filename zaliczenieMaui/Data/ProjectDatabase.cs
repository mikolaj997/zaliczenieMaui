using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;
using zaliczenieMaui.Models;

namespace zaliczenieMaui.Data
{
    internal class ProjectDatabase
    {
        
            readonly SQLiteAsyncConnection _database;

            public ProjectDatabase(string dbPath)
            {
                _database = new SQLiteAsyncConnection(dbPath);
                _database.CreateTableAsync<Project>().Wait();
            }

            public Task<List<Project>> GetProjectsAsync()
            {
                return _database.Table<Project>().ToListAsync();
            }

            public Task<Project> GetProjectAsync(int id)
            {
                return _database.Table<Project>().Where(i => i.Id == id).FirstOrDefaultAsync();
            }

            public Task<int> SaveProjectAsync(Project project)
            {
                if (project.Id != 0)
                {
                    return _database.UpdateAsync(project);
                }
                else
                {
                    return _database.InsertAsync(project);
                }
            }

            public Task<int> DeleteProjectAsync(Project project)
            {
                return _database.DeleteAsync(project);
            }
        
    }
}
