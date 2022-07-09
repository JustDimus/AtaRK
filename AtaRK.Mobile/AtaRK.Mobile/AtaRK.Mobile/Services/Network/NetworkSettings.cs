using System;
using System.Collections.Generic;
using System.Text;

namespace AtaRK.Mobile.Services.Network
{
    public class NetworkSettings
    {
        public string BaseURL { get; } = "https://9065-188-163-27-104.ngrok.io/api/";

        public bool UseRelativeUrls { get; } = true;
    }
}
