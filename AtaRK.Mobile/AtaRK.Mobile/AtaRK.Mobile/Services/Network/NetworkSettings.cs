using System;
using System.Collections.Generic;
using System.Text;

namespace AtaRK.Mobile.Services.Network
{
    public class NetworkSettings
    {
        public string BaseURL { get; } = "https://atarkwebapi.azurewebsites.net/api/";

        public bool UseRelativeUrls { get; } = true;
    }
}
