using System;
using System.Text;

namespace SimpleTokenAuthApp {
       public class User
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Token { get; set; }
    }
    
    public class TokenManager {
        public string GenerateToken(string username) {
            var expiry = DateTime.UtcNow.AddMinutes(30).ToString();
            string tokenData = $"{username}:{expiry}";
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(tokenData));
        }
    }
}