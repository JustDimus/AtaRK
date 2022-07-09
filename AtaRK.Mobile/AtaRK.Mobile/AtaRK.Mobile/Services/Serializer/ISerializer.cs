using System;
using System.Collections.Generic;
using System.Text;

namespace AtaRK.Mobile.Services.Serializer
{
    public interface ISerializer
    {
        string Serialize(object entity);

        object Deserialize(string data);

        T Deserialize<T>(string data);
    }
}
