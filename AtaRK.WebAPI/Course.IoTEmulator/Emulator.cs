using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ConfigEntity = Course.IoTEmulator.RequestResult<Course.IoTEmulator.ConfigResult>;

namespace Course.IoTEmulator
{
    public class Emulator
    {
        private class EmulatorMessageHandler : DelegatingHandler
        {
            private readonly Func<string> _getTokenFunction;

            public EmulatorMessageHandler(Func<string> getTokenFunction)
            {
                this._getTokenFunction = getTokenFunction;

                InnerHandler = new HttpClientHandler();
            }

            protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", this._getTokenFunction?.Invoke());

                return await base.SendAsync(request, cancellationToken);
            }
        }

        private const string BASE_API_URL = @"https://localhost:44389/api/";

        private readonly ILogger _logger;

        private readonly EmulatorMessageHandler _messageHandler;

        private readonly HttpClient _httpClient;

        private readonly Dictionary<string, Action<string>> _deviceSettingHandlers;

        public Emulator(ILogger logger)
        {
            this._logger = logger;

            this._messageHandler = new EmulatorMessageHandler(() => this.Token);

            this._httpClient = new HttpClient(handler: this._messageHandler)
            {
                BaseAddress = new Uri(BASE_API_URL)
            };

            this._deviceSettingHandlers = new Dictionary<string, Action<string>>()
            {
                {
                    "Enabled".ToUpperInvariant(), (value) =>
                    {
                        if (bool.TryParse(value, out var result))
                        {
                            this._logger.Log($"Devices status switched to {(result ? "Enabled" : "Disabled")}");
                            this._logger.SetColor(result ? ConsoleColor.Green : ConsoleColor.Red);
                        }
                        else
                        {
                            this._logger.Log("Invalid parameter");
                        }
                    }
                }
            };
        }

        private string Token { get; set; } = string.Empty;

        private string DeviceName { get; set; }

        private string DevicePassword { get; set; }

        private string DeviceId { get; set; }

        private string DeviceFullName => $"{this.DeviceName}@deviceproject.com";

        public void Start(string deviceName, string devicePassword, string deviceId)
        {
            this.DeviceName = deviceName;
            this.DevicePassword = devicePassword;
            this.DeviceId = deviceId;

            this._logger.SetColor(ConsoleColor.Yellow);

            while (true)
            {
                if (!this.DoAction())
                {
                    this._logger.SetColor(ConsoleColor.DarkRed);
                    this._logger.Log("Exit");
                    break;
                }

                Thread.Sleep(1000);
            }
        }

        public bool GetToken(int retryCount = 3, int currentTry = 0)
        {
            this._logger.Log($"{currentTry} try to get device token");

            var result = this._httpClient.PostAsync("account/login", JsonContent.Create(new
            {
                email = this.DeviceFullName,
                password = this.DevicePassword
            })).ConfigureAwait(false).GetAwaiter().GetResult();

            if (result.StatusCode == System.Net.HttpStatusCode.OK)
            {
                this._logger.Log("Device received the token");

                var tokenResult = JsonConvert.DeserializeObject<AccessTokenResult>(result.Content.ReadAsStringAsync().ConfigureAwait(false).GetAwaiter().GetResult());

                this.Token = tokenResult.Token;

                return true;
            }

            this._logger.Log($"Error happened: {result.StatusCode}");

            if (retryCount == currentTry)
            {
                return false;
            }

            return GetToken(retryCount, currentTry + 1);
        }

        private bool DoAction()
        {
            this._logger.Log("Tick start");

            if (string.IsNullOrEmpty(this.Token))
            {
                this._logger.SetColor(ConsoleColor.White);
                this._logger.Log("Device hasn't registered yet");

                if (!this.GetToken(3))
                {
                    this._logger.Log("Device couldn't register the on cloud");
                    return false;
                }

                this._logger.SetColor(ConsoleColor.Yellow);
            }

            this._logger.Log("Receiving current configuration");

            var configurationResult = this.GetCloudConfiguration();

            if (configurationResult.Status)
            {
                this._logger.Log("Device configuration successfully received");

                this.OperateDeviceConfiguration(configurationResult.Value);
            }
            else
            {
                this._logger.Log("Failed to receive cloud configuration");
            }

            this._logger.Log("Tick end");

            return true;
        }

        private ConfigEntity GetCloudConfiguration(bool doRetry = true)
        {
            this._logger.Log("Receiving device configuration");

            var result = this._httpClient.PostAsync("device/myconfig", JsonContent.Create(new
            {
                body = this.DeviceId
            })).ConfigureAwait(false).GetAwaiter().GetResult();

            if (result.StatusCode == System.Net.HttpStatusCode.OK)
            {
                this._logger.Log("Device received the cloud configuration");

                var requestResult = result.Content.ReadAsStringAsync().ConfigureAwait(false).GetAwaiter().GetResult();

                this._logger.Log(requestResult);

                var entityResult = JsonConvert.DeserializeObject<ConfigResult>(requestResult);

                return new ConfigEntity()
                {
                    Status = true,
                    Value = entityResult
                };
            }

            this._logger.Log($"Failed to receive device configuration: {result.StatusCode}");

            if (!doRetry)
            {
                return new ConfigEntity();
            }

            if (result.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                if (!this.GetToken())
                {
                    return new ConfigEntity();
                }

                this._logger.Log("Retry receiving device configuration");

                return GetCloudConfiguration(false);
            }

            this._logger.Log($"Error happened: {result.StatusCode}");
            return new ConfigEntity();
        }

        private void OperateDeviceConfiguration(ConfigResult config)
        {
            this._logger.Log("Operating device configuration");

            foreach (var i in config.Settings)
            {
                if (this._deviceSettingHandlers.ContainsKey(i.Setting.ToUpperInvariant()))
                {
                    this._deviceSettingHandlers[i.Setting.ToUpperInvariant()]?.Invoke(i.Value);
                }
                else
                {
                    this._logger.Log($"Command '{i.Setting.ToUpperInvariant()}' is not supported");
                }
            }

            this._logger.Log("Operation device configuration failed");
        }
    }

    public class AccessTokenResult
    {
        [JsonProperty("access_token")]
        public string Token { get; set; }
    }

    public class ConfigResult
    {
        [JsonProperty("list")]
        public List<SettingResult> Settings { get; set; }
    }

    public class SettingResult
    {
        [JsonProperty("setting")]
        public string Setting { get; set; }

        [JsonProperty("value")]
        public string Value { get; set; }
    }

    public class RequestResult<TEntity>
    {
        public bool Status { get; set; } = false;

        public TEntity Value { get; set; } = default;
    }
}
