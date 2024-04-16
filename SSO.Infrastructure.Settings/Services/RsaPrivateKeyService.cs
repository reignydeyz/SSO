using System.Security.Cryptography;

namespace SSO.Infrastructure.Settings.Services
{
    public class RsaPrivateKeyService
    {
        /// <summary>
        /// Creates private key
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public RSA CreatePrivateKey(string path)
        {
            if (!File.Exists(path))
            {
                var privateKey = RSA.Create();
                string privateKeyPem = $"-----BEGIN PRIVATE KEY-----\n{Convert.ToBase64String(privateKey.ExportPkcs8PrivateKey())}\n-----END PRIVATE KEY-----\n";
                File.WriteAllText(path, privateKeyPem);
                Console.WriteLine("Private key PEM file created successfully.");
                return privateKey;
            }
            else
            {
                string pemContents = File.ReadAllText(path);
                var privateKey = RSA.Create();
                privateKey.ImportFromPem(pemContents);
                Console.WriteLine("Private key PEM file already exists.");
                return privateKey;
            }
        }
    }
}
