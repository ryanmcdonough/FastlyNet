using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace FastlyNet
{
    public class Fastly
    {
        private static HttpClient _client;
        private const string EndPoint = "https://api.fastly.com";

        public Fastly(string fastlyApiKey)
        {
            _client = new HttpClient { BaseAddress = new Uri(EndPoint) };

            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _client.DefaultRequestHeaders.Add("Fastly-Key", fastlyApiKey);
        }

        public Task Purge(string uri)
        {
            return Purge(new Uri(uri));
        }

        public Task Purge(Uri uri)
        {
            return SendRequest("PURGE", uri);
        }

        public Task PurgeKey(string service, string key)
        {
            var uri = $"{EndPoint}/service/{WebUtility.UrlEncode(service)}/purge/{key}";

            return SendRequest("POST", new Uri(uri));
        }

        public Task PurgeAll(string service)
        {
            var uri = $"{EndPoint}/service/{WebUtility.UrlEncode(service)}/purge_all";

            return SendRequest("POST", new Uri(uri));
        }

        private Task SendRequest(string method, Uri requestUri)
        {
            var request = new HttpRequestMessage
            {
                Method = new HttpMethod(method),
                RequestUri = requestUri
            };            

            var response = _client.SendAsync(request);

            return response;
        }
    }
}
