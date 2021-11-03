using AtaRK.BLL.Interfaces;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace AtaRK.BLL.Implementations
{
    public class EncryptionService : IEncryptionService
    {
        private const string ENCRYPTION_KEY = "BHTEU/EEHcA8RBbWI0r/7zgPZsn6xtHqRj7X8qkeosA=";
        private const string INITIALIZATION_VECTOR = "UF6avPUNcDxnVBYg+MbSPw==";

        public string Encrypt(string data)
        {
            if (string.IsNullOrEmpty(data))
            {
                throw new ArgumentException($"{nameof(data)} is null or empty");
            }

            using (Aes aes = Aes.Create())
            {
                aes.IV = Convert.FromBase64String(INITIALIZATION_VECTOR);
                aes.Key = Convert.FromBase64String(ENCRYPTION_KEY);
                ICryptoTransform encryptor = aes.CreateEncryptor();

                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cryptoStream = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter writer = new StreamWriter(cryptoStream, Encoding.Unicode))
                        {
                            writer.Write(data);
                        }
                    }

                    return Convert.ToBase64String(ms.ToArray());
                }
            }
        }

        public string Decrypt(string data)
        {
            if (string.IsNullOrEmpty(data))
            {
                throw new ArgumentException($"{nameof(data)} is null or empty");
            }

            using (Aes aes = Aes.Create())
            {
                aes.IV = Convert.FromBase64String(INITIALIZATION_VECTOR);
                aes.Key = Convert.FromBase64String(ENCRYPTION_KEY);
                ICryptoTransform decryptor = aes.CreateDecryptor();

                using (MemoryStream ms = new MemoryStream(Convert.FromBase64String(data)))
                {
                    using (CryptoStream cryptoStream = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader reader = new StreamReader(cryptoStream, Encoding.Unicode))
                        {
                            return reader.ReadLine();
                        }
                    }
                }
            }
        }

        public string Hash(string data)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] sourceArray = Encoding.UTF8.GetBytes(data);

                byte[] hash = sha256Hash.ComputeHash(sourceArray);

                return Encoding.UTF8.GetString(hash);
            }
        }

        public bool CompareWithHash(string data, string hash)
        {
            return this.Hash(data) == hash;
        }
    }
}
