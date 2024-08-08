using MySql.Data.MySqlClient;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;

class Program
{
    static string connectionString;

    static void Main(string[] args)
    {
        LoadConfiguration();
        Console.WriteLine("CRUD Application with C# and MySQL");
        // CRUD operations here

        // Create User
        CreateUser("John Doe", "john.doe@example.com");

        // Read Users
        var users = GetUsers();
        foreach (var user in users)
        {
            Console.WriteLine($"ID: {user.Id}, Name: {user.Name}, Email: {user.Email}");
        }

        // Update User
        UpdateUser(1, "Jane Doe", "jane.doe@example.com");

        // Delete User
        DeleteUser(1);
    }

    static void LoadConfiguration()
    {
        var config = JObject.Parse(File.ReadAllText("appsettings.json"));
        connectionString = config["ConnectionStrings"]["DefaultConnection"].ToString();
    }

    static void CreateUser(string name, string email)
    {
        using (var connection = new MySqlConnection(connectionString))
        {
            connection.Open();
            var query = "INSERT INTO users (name, email) VALUES (@name, @Email)";
            using (var command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@name", name);
                command.Parameters.AddWithValue("@Email", email);
                command.ExecuteNonQuery();
            }
        }
    }

    static List<User> GetUsers()
    {
        var users = new List<User>();
        using (var connection = new MySqlConnection(connectionString))
        {
            connection.Open();
            var query = "SELECT * FROM users";
            using (var command = new MySqlCommand(query, connection))
            {
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        users.Add(new User
                        {
                            Id = reader.GetInt32("id"),
                            Name = reader.GetString("name"),
                            Email = reader.GetString("email")
                        });
                    }
                }
            }
        }
        return users;
    }

    static void UpdateUser(int id, string name, string email)
    {
        using (var connection = new MySqlConnection(connectionString))
        {
            connection.Open();
            var query = "UPDATE users SET name = @name, email = @Email WHERE id = @id";
            using (var command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@id", id);
                command.Parameters.AddWithValue("@name", name);
                command.Parameters.AddWithValue("@Email", email);
                command.ExecuteNonQuery();
            }
        }
    }

    static void DeleteUser(int id)
    {
        using (var connection = new MySqlConnection(connectionString))
        {
            connection.Open();
            var query = "DELETE FROM users WHERE id = @id";
            using (var command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@id", id);
                command.ExecuteNonQuery();
            }
        }
    }

    class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
    }
}
