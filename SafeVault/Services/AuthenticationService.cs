using BCrypt.Net;
using Microsoft.Extensions.Options;
using MySql.Data.MySqlClient;

public class AuthenticationService
{
    private readonly string _connectionString;

    public AuthenticationService() {}

    public AuthenticationService(IOptions<DatabaseSettings> databaseOption)
    {
        _connectionString = databaseOption.Value.MySqlConnection;
    }

    public void ClearTest()
    {
        // Очищення тестових користувачів
        using var connection = new MySqlConnection(_connectionString);
        connection.Open();

        using var cmd = new MySqlCommand("DELETE FROM users", connection);
        cmd.ExecuteNonQuery();
    }

    public void Register(string username, string email, string password, string role)
    {
        string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);

        using var conn = new MySqlConnection(_connectionString);
        conn.Open();

        var query = "INSERT INTO Users (Username, Email, PasswordHash, Role) VALUES (@username, @email, @passwordHash, @role)";
        using var cmd = new MySqlCommand(query, conn);
        cmd.Parameters.AddWithValue("@username", username);
        cmd.Parameters.AddWithValue("@email", email);
        cmd.Parameters.AddWithValue("@passwordHash", hashedPassword);
        cmd.Parameters.AddWithValue("@role", role);

        cmd.ExecuteNonQuery();
    }

    public bool Authenticate(string username, string password)
    {
        using var conn = new MySqlConnection(_connectionString);
        conn.Open();

        var query = "SELECT PasswordHash FROM Users WHERE Username = @username";
        using var cmd = new MySqlCommand(query, conn);
        cmd.Parameters.AddWithValue("@username", username);

        using var reader = cmd.ExecuteReader();
        if (reader.Read())
        {
            string storedHash = reader.GetString(0);
            return BCrypt.Net.BCrypt.Verify(password, storedHash);
        }

        return false;
    }

    public string GetUserRole(string username)
    {
        using var conn = new MySqlConnection(_connectionString);
        conn.Open();

        var query = "SELECT Role FROM Users WHERE Username = @username";
        using var cmd = new MySqlCommand(query, conn);
        cmd.Parameters.AddWithValue("@username", username);

        using var reader = cmd.ExecuteReader();
        if (reader.Read())
        {
            return reader.GetString(0);
        }

        return null;
    }

}
