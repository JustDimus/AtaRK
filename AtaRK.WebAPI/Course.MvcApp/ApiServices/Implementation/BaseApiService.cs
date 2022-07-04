using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Course.MvcApp.ApiServices.Implementation
{
    public class BaseApiService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public BaseApiService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
        }

        protected async Task<TResult> SendRequestAsync<TResult>(string requestAddress, RequestMethod requestMethod, object body = null) where TResult : class
        {
            var httpClient = _httpClientFactory.CreateClient("CourseAPIClient");

            HttpResponseMessage responseMessage = null;

            switch (requestMethod)
            {
                case RequestMethod.GET:
                    responseMessage = await httpClient.GetAsync(requestAddress);
                    break;
                case RequestMethod.POST:
                    responseMessage = await httpClient.PostAsync(requestAddress, JsonContent.Create(body));
                    break;
            }

            if (responseMessage.StatusCode != System.Net.HttpStatusCode.OK)
            {
                return null;
            }

            var contentResult = await responseMessage.Content.ReadAsStringAsync();

            var resultObject = JsonConvert.DeserializeObject<TResult>(contentResult);

            return resultObject;
        }

        protected async Task<bool> SendRequestWithoutResponseAsync(string requestAddress, RequestMethod requestMethod, object body = null)
        {
            var httpClient = _httpClientFactory.CreateClient("CourseAPIClient");

            HttpResponseMessage responseMessage = null;

            switch (requestMethod)
            {
                case RequestMethod.GET:
                    responseMessage = await httpClient.GetAsync(requestAddress);
                    break;
                case RequestMethod.POST:
                    responseMessage = await httpClient.PostAsync(requestAddress, JsonContent.Create(body));
                    break;
            }

            return responseMessage.StatusCode == System.Net.HttpStatusCode.OK;
        }
    }
}
