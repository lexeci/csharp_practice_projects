using System;
using System.Security.Cryptography;
using System.Text;

namespace SecureDataApp
{
    public class SecureStorage
    {
        private string _encryptedData;

        public void StoreData(string data, byte[] key, byte[] iv)
        {
            _encryptedData = Convert.ToBase64String(Encrypt(data, key, iv));
        }

        public string RetrieveData(User user, byte[] key, byte[] iv)
        {
            if (user.Role != "Admin")
                throw new UnauthorizedAccessException("Access denied. Admin role required.");

            return Decrypt(Convert.FromBase64String(_encryptedData), key, iv);
        }

        private static byte[] Encrypt(string data, byte[] key, byte[] iv)
        {
            using (Aes aes = Aes.Create())
            {
                aes.Key = key;
                aes.IV = iv;
                ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

                using (var ms = new System.IO.MemoryStream())
                using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                {
                    using (var writer = new System.IO.StreamWriter(cs))
                    {
                        writer.Write(data);
                    }
                    return ms.ToArray();
                }
            }
        }

        private static string Decrypt(byte[] data, byte[] key, byte[] iv)
        {
            using (Aes aes = Aes.Create())
            {
                aes.Key = key;
                aes.IV = iv;
                ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

                using (var ms = new System.IO.MemoryStream(data))
                using (var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                using (var reader = new System.IO.StreamReader(cs))
                {
                    return reader.ReadToEnd();
                }
            }
        }
    }
}