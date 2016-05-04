using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace FastlyNet
{
    public class Fastly
    {
        public Fastly(string fastlyApiKey)
        {
            _fastlyApiKey = fastlyApiKey;
        }

        private readonly string _fastlyApiKey;

        private const string EndPoint = "https://api.fastly.com";

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

            request.Headers.Add("Accept", "application/json");
            request.Headers.Add("Fastly-Key", _fastlyApiKey);

            var client = new HttpClient();

            var response = client.SendAsync(request);

            return response;
        }
    }
}
