using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace AtaRK.Utility.Json
{
    public static class SerializationSettings
    {
        public static JsonSerializerSettings Instance
        {
            get
            {
                return new JsonSerializerSettings()
                {

                };
            }
        }
    }
}
