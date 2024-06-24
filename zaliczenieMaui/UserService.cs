//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System.Data.SqlClient;
//using System.Threading;
//using System.Threading.Tasks;


//namespace zaliczenieMaui
//{
  
//    public static class UserService
//    {
//        private const string server = "your_server_name";
//        private const string database = "your_database_name";
//        private const string username = "your_username";
//        private const string password = "your_password";

//        private static string connectionString => $"Data Source={server};Initial Catalog={database};User ID={username};Password={password}";

//        public static async Task<UserDto> GetUserAsync(string username, string password, CancellationToken cancellationToken)
//        {
//            try
//            {
//                using (SqlConnection connection = new SqlConnection(connectionString))
//                {
//                    await connection.OpenAsync(cancellationToken);

//                    string sql = "SELECT * FROM Users WHERE Username = @username AND Password = @password";
//                    using (SqlCommand command = new SqlCommand(sql, connection))
//                    {
//                        command.Parameters.AddWithValue("@username", username);
//                        command.Parameters.AddWithValue("@password", password);

//                        using (SqlDataReader reader = await command.ExecuteReaderAsync(cancellationToken))
//                        {
//                            if (await reader.ReadAsync(cancellationToken))
//                            {
//                                return new UserDto
//                                {
//                                    Id = reader.GetInt32(reader.GetOrdinal("Id")),
//                                    Username = reader.GetString(reader.GetOrdinal("Username")),
//                                    Email = reader.GetString(reader.GetOrdinal("Email")),
//                                    // Dodaj inne właściwości użytkownika, które chcesz pobrać
//                                };
//                            }
//                        }
//                    }
//                }
//            }
//            catch (Exception ex)
//            {
//                // Obsługa błędów połączenia lub zapytania SQL
//                Console.WriteLine($"Error retrieving user: {ex.Message}");
//            }

//            return null;
//        }
//    }

//}
