using Microsoft.Extensions.Options;
using MySql.Data.MySqlClient;

public class DatabaseService
{
    private readonly string _connectionString;

    public DatabaseService() {}

    public DatabaseService(IOptions<DatabaseSettings> databaseOption)
    {
        _connectionString = databaseOption.Value.MySqlConnection;
    }

    public void InsertUser(string username, string email, string passwordHash)
    {
        using var conn = new MySqlConnection(_connectionString);
        conn.Open();

        string query = "INSERT INTO Users (Username, Email, PasswordHash) VALUES (@username, @email, @PasswordHash)";
        using var cmd = new MySqlCommand(query, conn);
        cmd.Parameters.AddWithValue("@username", username);
        cmd.Parameters.AddWithValue("@email", email);
        cmd.Parameters.AddWithValue("@PasswordHash", passwordHash);

        cmd.ExecuteNonQuery();
    }
}
