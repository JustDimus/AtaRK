using System;
using System.Collections.Generic;
using System.Text;

namespace AtaRK.BLL.Interfaces
{
    public interface IEncryptionService
    {
        string Encrypt(string data);

        string Decrypt(string data);

        string Hash(string data);

        bool CompareWithHash(string data, string hash);
    }
}
