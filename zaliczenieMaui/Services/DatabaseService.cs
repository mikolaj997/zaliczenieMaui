using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SQLite;
using zaliczenieMaui.Models;
using System.Threading.Tasks;

namespace zaliczenieMaui.Services
{
    public class DatabaseService
    {
        private SQLiteAsyncConnection _database;

        public DatabaseService(string dbPath)
        {
            /*if (File.Exists(dbPath))
            {
                File.Delete(dbPath);
            }*/

            _database = new SQLiteAsyncConnection(dbPath);
            _database.CreateTableAsync<User>().Wait();
            _database.CreateTableAsync<Project>().Wait();
            _database.CreateTableAsync<ProjectTask>().Wait();
            _database.CreateTableAsync<ProjectMember>().Wait();
            _database.CreateTableAsync<Comment>().Wait();
        }

        // User Methods
        public Task<User> GetUserAsync(string email, string password)
        {
            return _database.Table<User>().Where(u => u.Email == email && u.Password == password).FirstOrDefaultAsync();
        }

        public Task<int> SaveUserAsync(User user)
        {
            return _database.InsertAsync(user);
        }
        public Task<User> GetUserByEmailAsync(string email)
        {
            return _database.Table<User>().Where(u => u.Email == email).FirstOrDefaultAsync();
        }
        // Project Methods
        public Task<List<Project>> GetAllProjectsAsync()
        {
            return _database.Table<Project>().ToListAsync();
        }

        public Task<List<Project>> GetProjectsByOwnerAsync(string email)
        {
            return _database.Table<Project>().Where(p => p.OwnerEmail == email).ToListAsync();
        }

        public Task<int> SaveProjectAsync(Project project)
        {
            return _database.InsertAsync(project);
        }

        public Task<int> UpdateProjectAsync(Project project)
        {
            return _database.UpdateAsync(project);
        }

        // Task Methods
        public Task<List<ProjectTask>> GetTasksByProjectAsync(int projectId)
        {
            return _database.Table<ProjectTask>().Where(t => t.ProjectId == projectId).ToListAsync();
        }

        public Task<int> SaveTaskAsync(ProjectTask task)
        {
            return _database.InsertAsync(task);
        }

        public Task<int> UpdateTaskAsync(ProjectTask task)
        {
            return _database.UpdateAsync(task);
        }
        // Project Member Methods
        public Task<List<ProjectMember>> GetMembersByProjectAsync(int projectId)
        {
            return _database.Table<ProjectMember>().Where(pm => pm.ProjectId == projectId).ToListAsync();
        }

        public Task<int> AddMemberToProjectAsync(ProjectMember projectMember)
        {
            return _database.InsertAsync(projectMember);
        }

        public Task<int> RemoveMemberFromProjectAsync(ProjectMember projectMember)
        {
            return _database.DeleteAsync(projectMember);
        }

        public Task<List<Project>> GetProjectsByMemberAsync(string email)
        {
            return _database.QueryAsync<Project>(
                "SELECT p.* FROM Project p INNER JOIN ProjectMember pm ON p.Id = pm.ProjectId WHERE pm.MemberEmail = ?", email);
        }
        // Comment Methods
        public Task<List<Comment>> GetCommentsByProjectIdAsync(int projectId)
        {
            return _database.Table<Comment>().Where(c => c.ProjectId == projectId).ToListAsync();
        }

        public Task<int> SaveCommentAsync(Comment comment)
        {
            return _database.InsertAsync(comment);
        }

    }
}
