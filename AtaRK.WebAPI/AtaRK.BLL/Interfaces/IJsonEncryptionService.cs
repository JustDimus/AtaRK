using System;
using System.Collections.Generic;
using System.Text;

namespace AtaRK.BLL.Interfaces
{
    public interface IJsonEncryptionService
    {
        TEntity Decrypt<TEntity>(string data);

        string Encrypt<TEntity>(TEntity entity);
    }
}
