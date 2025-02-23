using System.Security.Cryptography;
using System.Text;

namespace SSO.Infrastructure.Settings.Services
{
    public class RsaKeyService
    {
        /// <summary>
        /// Creates private key
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public RSA CreatePrivateKey(string path)
        {
            var privateKey = RSA.Create();

            if (!File.Exists(path))
            {
                File.WriteAllText(path, $"-----BEGIN PRIVATE KEY-----\n{Convert.ToBase64String(privateKey.ExportPkcs8PrivateKey())}\n-----END PRIVATE KEY-----\n");
                Console.WriteLine("Private key PEM file created successfully.");
            }
            else
            {
                privateKey.ImportFromPem(File.ReadAllText(path));
                Console.WriteLine("Private key PEM file already exists.");
            }

            return privateKey;
        }

        /// <summary>
        /// Creates the public key from the private key, saves it to a PEM file, and returns the public RSA key instance.
        /// </summary>
        public RSA CreatePublicKey(RSA privateKey, string publicKeyPath)
        {
            // Export the public key from the private key
            var publicKey = privateKey.ExportSubjectPublicKeyInfo();

            // Convert the public key to PEM format
            string publicKeyPem = $"-----BEGIN PUBLIC KEY-----\n{Convert.ToBase64String(publicKey)}\n-----END PUBLIC KEY-----\n";

            // Save the public key to the specified file if it doesn't already exist
            if (!File.Exists(publicKeyPath))
            {
                File.WriteAllText(publicKeyPath, publicKeyPem);
            }

            // Create RSA instance for the public key
            RSA rsaPublicKey = RSA.Create();
            rsaPublicKey.ImportSubjectPublicKeyInfo(publicKey, out _);

            // Return the RSA public key object
            return rsaPublicKey;
        }

        /// <summary>
        /// Encrypts using the public key
        /// </summary>
        public string EncryptString(string input, RSA publicKey)
        {
            byte[] inputBytes = Encoding.UTF8.GetBytes(input);
            byte[] encryptedBytes = publicKey.Encrypt(inputBytes, RSAEncryptionPadding.OaepSHA256);
            return Convert.ToBase64String(encryptedBytes); // Store in NVARCHAR(500)
        }

        /// <summary>
        /// Decrypts an encrypted Base64 string using the private key.
        /// </summary>
        public string DecryptString(string encryptedBase64, RSA privateKey)
        {
            byte[] encryptedBytes = Convert.FromBase64String(encryptedBase64);
            byte[] decryptedBytes = privateKey.Decrypt(encryptedBytes, RSAEncryptionPadding.OaepSHA256);
            return Encoding.UTF8.GetString(decryptedBytes);
        }
    }
}
