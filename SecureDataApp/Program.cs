using System;

namespace SecureDataApp
{
    class Program
    {
        static void Main(string[] args)
        {
            // Create users
            var admin = new User { Username = "AdminUser", Role = "Admin" };
            var user = new User { Username = "BasicUser", Role = "User" };

            // Initialize SecureStorage and set up encryption
            var storage = new SecureStorage();
            byte[] encryptionKey;
            byte[] initializationVector;

            using (var aes = System.Security.Cryptography.Aes.Create())
            {
                aes.GenerateKey();
                aes.GenerateIV();
                encryptionKey = aes.Key;
                initializationVector = aes.IV;

                // Store encrypted data
                storage.StoreData("Sensitive Information", aes.Key, aes.IV);
            }

            // Attempt to retrieve data as Admin
            try
            {
                string adminData = storage.RetrieveData(admin, encryptionKey, initializationVector);
                Console.WriteLine($"Admin Access: {adminData}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Admin Error: {ex.Message}");
            }

            // Attempt to retrieve data as Basic User
            try
            {
                string userData = storage.RetrieveData(user, encryptionKey, initializationVector);
                Console.WriteLine($"User Access: {userData}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"User Error: {ex.Message}");
            }
        }
    }
}