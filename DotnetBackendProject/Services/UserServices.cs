using System.Collections.Generic;

public class UserService
{
    private readonly List<User> _users = new();

    public IEnumerable<User> GetAllUsers() => _users;

    public bool ValidateUser(User user)
    {
        return !(string.IsNullOrWhiteSpace(user.Name) || string.IsNullOrWhiteSpace(user.Email));
    }

    public void AddUser(User user) => _users.Add(user);

    public bool UpdateUser(int id, User user)
    {
        var existingUser = _users.Find(u => u.Id == id);
        if (existingUser == null) return false;
        existingUser.Name = user.Name;
        existingUser.Email = user.Email;
        return true;
    }

    public bool DeleteUser(int id)
    {
        var user = _users.Find(u => u.Id == id);
        return user != null && _users.Remove(user);
    }
}
