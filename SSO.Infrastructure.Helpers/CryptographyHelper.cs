using System.Security.Cryptography;

namespace SSO.Infrastructure.Helpers
{
    public static class CryptographyHelper
    {
        public static byte[] GenerateKey(int? keySize = 256)
        {
            using (Aes aes = Aes.Create())
            {
                aes.KeySize = keySize!.Value;
                aes.GenerateKey();
                return aes.Key;
            }
        }

        public static byte[] EncryptStringToBytes_Aes(string plainText, byte[] Key)
        {
            using (Aes aes = Aes.Create())
            {
                aes.Key = Key;
                aes.GenerateIV(); // Generate a random IV
                byte[] iv = aes.IV;

                using (var encryptor = aes.CreateEncryptor(aes.Key, iv))
                using (var ms = new MemoryStream())
                {
                    // Prepend the IV to the ciphertext
                    ms.Write(iv, 0, iv.Length); // Write the IV first

                    using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                    using (var sw = new StreamWriter(cs))
                    {
                        sw.Write(plainText);
                    }

                    return ms.ToArray(); // Return the complete byte array
                }
            }
        }

        public static string DecryptStringFromBytes_Aes(byte[] cipherText, byte[] Key)
        {
            using (Aes aes = Aes.Create())
            {
                aes.Key = Key;

                // Extract the IV from the beginning of the ciphertext
                byte[] iv = new byte[16]; // AES block size
                Array.Copy(cipherText, 0, iv, 0, iv.Length);

                using (var decryptor = aes.CreateDecryptor(aes.Key, iv))
                using (var ms = new MemoryStream(cipherText, iv.Length, cipherText.Length - iv.Length))
                using (var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                using (var sr = new StreamReader(cs))
                {
                    return sr.ReadToEnd();
                }
            }
        }
    }
}
