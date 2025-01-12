using ManageUsers.Models;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Security.Cryptography;
using System.Text;

namespace MangerVault.WebAPIServices.UtilityHelper
{
    public class PasswordHelper
    {
        // Hash the password
        public static (string hashedPassword, string salt) HashPassword(string password, string? passphrase=default)
        {
            byte[]? salt;
            if (string.IsNullOrEmpty(passphrase))
            {
                // Generate a 128-bit salt using a sequence of cryptographically strong random bytes.
                salt = RandomNumberGenerator.GetBytes(128 / 8); // divide by 8 to convert bits to bytes
                Console.WriteLine($"Salt: {Convert.ToBase64String(salt)}");
            }
            else
            {
                salt = Convert.FromBase64String(passphrase);
                //salt = Encoding.UTF8.GetBytes(passphrase);
            }
      
            // derive a 256-bit subkey (use HMACSHA256 with 100,000 iterations)
            string hashedPassword = Convert.ToBase64String(KeyDerivation.Pbkdf2(
            password: password!,
            salt: salt,
            prf: KeyDerivationPrf.HMACSHA256,
            iterationCount: 100000,
            numBytesRequested: 256 / 8));

            return (hashedPassword, Convert.ToBase64String(salt));
        }

        // Verify the password
        public static bool VerifyPassword(string loginPassword, Credential dbCredential)
        {
            return HashPassword(loginPassword, dbCredential.Passphrase).hashedPassword == dbCredential.Password;
        }

        // Method to encrypt a password using AES
        public static string EncryptPassword(string password, string key, string iv)
        {
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Encoding.UTF8.GetBytes(key); // 16 bytes for AES-128
                aesAlg.IV = Encoding.UTF8.GetBytes(iv); // 16 bytes for AES block size

                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);
                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                    {
                        swEncrypt.Write(password);
                    }
                    return Convert.ToBase64String(msEncrypt.ToArray());
                }
            }
        }

        // Method to decrypt a password using AES
        public static string DecryptPassword(string encryptedPassword, string key, string iv)
        {
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Encoding.UTF8.GetBytes(key); // 16 bytes for AES-128
                aesAlg.IV = Encoding.UTF8.GetBytes(iv); // 16 bytes for AES block size

                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);
                using (MemoryStream msDecrypt = new MemoryStream(Convert.FromBase64String(encryptedPassword)))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                    {
                        return srDecrypt.ReadToEnd();
                    }
                }
            }
        }

        // Method to securely encrypt a password using AES
        public static string EncryptPassword(string password, string key)
        {
            using (Aes aesAlg = Aes.Create())
            {
                // Generate a random 16-byte initialization vector (IV)
                aesAlg.GenerateIV();

                // Set the key and IV (ensure the key is correct length: 16, 24, or 32 bytes for AES)
                aesAlg.Key = Encoding.UTF8.GetBytes(key); // Key must be 16, 24, or 32 bytes long
                aesAlg.IV = aesAlg.IV; // Use the generated IV

                // Create encryptor
                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                // Create memory stream to hold the encrypted password
                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    // Create CryptoStream to perform the encryption
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                    {
                        // Write the password into the CryptoStream (this encrypts the data)
                        swEncrypt.Write(password);
                    }

                    // Get the encrypted data (ciphertext) as a base64 string
                    byte[] encryptedPassword = msEncrypt.ToArray();
                    // Combine the IV with the encrypted password for storage
                    byte[] result = new byte[aesAlg.IV.Length + encryptedPassword.Length];
                    Array.Copy(aesAlg.IV, 0, result, 0, aesAlg.IV.Length);
                    Array.Copy(encryptedPassword, 0, result, aesAlg.IV.Length, encryptedPassword.Length);
                    return Convert.ToBase64String(result);
                }
            }
        }

        // Method to securely decrypt a password using AES
        public static string DecryptPassword(string encryptedPasswordBase64, string key)
        {
            // Convert the base64 string back to a byte array
            byte[] cipherTextBytesWithIv = Convert.FromBase64String(encryptedPasswordBase64);

            // Extract the IV and encrypted password from the byte array
            byte[] iv = new byte[16];
            byte[] cipherTextBytes = new byte[cipherTextBytesWithIv.Length - iv.Length];
            Array.Copy(cipherTextBytesWithIv, 0, iv, 0, iv.Length);
            Array.Copy(cipherTextBytesWithIv, iv.Length, cipherTextBytes, 0, cipherTextBytes.Length);

            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Encoding.UTF8.GetBytes(key); // The same key used for encryption
                aesAlg.IV = iv; // The extracted IV

                // Create decryptor
                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                // Perform decryption
                using (MemoryStream msDecrypt = new MemoryStream(cipherTextBytes))
                using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                {
                    return srDecrypt.ReadToEnd(); // Return the decrypted password
                }
            }
        }

        private static readonly string Characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        private static readonly Random Random = new Random();

        public static string GenerateRandomString(int length = 16)
        {
            char[] randomChars = new char[length];
            for (int i = 0; i < length; i++)
            {
                randomChars[i] = Characters[Random.Next(Characters.Length)];
            }
            return new string(randomChars);
        }
    }
}


