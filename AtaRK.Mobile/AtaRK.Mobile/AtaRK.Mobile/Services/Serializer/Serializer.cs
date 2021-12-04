using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace AtaRK.Mobile.Services.Serializer
{
    public class Serializer : ISerializer
    {
        public object Deserialize(string data)
        {
            try
            {
                return JsonConvert.DeserializeObject(data);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public T Deserialize<T>(string data)
        {
            try
            {
                return JsonConvert.DeserializeObject<T>(data);
            }
            catch (Exception)
            {
                return default(T);
            }
        }

        public string Serialize(object entity)
        {
            try
            {
                return JsonConvert.SerializeObject(entity);
            }
            catch(Exception)
            {
                return null;
            }
        }
    }
}
