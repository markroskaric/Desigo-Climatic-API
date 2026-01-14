using System.Text;
using System.Net.Http.Headers;

namespace ClimatixRestApi
{
    public class Connection
    {
        internal readonly string _baseUrl;
        internal readonly string _pin;
        private readonly string _authHeaderValue;
        internal readonly bool _dev;
        private readonly int _us;
        private readonly string _lng;
        private readonly HttpClient _client;
        internal string GetBaseUrl()
        {
            return _baseUrl;
        }
        internal string GetAuthHeaderValue()
        {
            return _authHeaderValue;
        }
        public Connection(string username, string password, string ip, string pin, bool dev = false, int us = 0, string lng = "0")
        {
            if (us < 0 || us > 4)
            {
                throw new ArgumentOutOfRangeException(nameof(us), "The 'us' parameter must be a number between 0 and 4.");
            }
            _baseUrl = $"{ip}/JSONGEN.HTML?FN=";
            _pin = pin;
            var authString = $"{username}:{password}";
            _authHeaderValue = Convert.ToBase64String(Encoding.UTF8.GetBytes(authString));
            _dev = dev;
            _us = us;
            _lng = lng;
            _client = new() { Timeout = TimeSpan.FromSeconds(15) };
        }
        internal ApiResponse SendRequest(string url)
        {
            var result = new ApiResponse();
            try
            {
                using (var request = new HttpRequestMessage(HttpMethod.Get, url))
                {
                    request.Headers.Authorization = new AuthenticationHeaderValue("Basic", _authHeaderValue);

                    using (var response = _client.SendAsync(request).GetAwaiter().GetResult())
                    {
                        result.StatusCode = (int)response.StatusCode;
                        result.IsSuccess = response.IsSuccessStatusCode;
                        result.Content = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                        return result;
                    }
                }
            }
            catch (Exception e)
            {
                result.IsSuccess = false;
                result.ErrorMessage = e.Message;
                return result;
            }
        }
    }
}