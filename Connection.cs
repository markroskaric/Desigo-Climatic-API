using System.Text;
using System.Net.Http.Headers;

namespace DesigoClimatixApi
{
    public class Connection 
    {
        private readonly string _baseUrl;
        private readonly string _pin;
        private readonly string _authHeaderValue;
        private readonly bool _dev = false;
        private readonly HttpClient _client ;

        internal string GetBaseUrl()
        {
            return _baseUrl;
        }
        internal string GetAuthHeaderValue()
        {
            return _authHeaderValue;
        }
        public Connection(string username, string password, string ip, string pin) 
            : this(username, password, ip, pin, false) 
        {

        }
        public Connection(string username, string password, string ip, string pin, bool dev)
        {
            _baseUrl = $"{ip}/JSONGEN.HTML?FN=";
            _pin = pin;
            var authString = $"{username}:{password}";
            _authHeaderValue = Convert.ToBase64String(Encoding.UTF8.GetBytes(authString));
            _dev = dev;
            _client = new() { Timeout = TimeSpan.FromSeconds(15) };
        }
        public object ReadValue(string base64Id)
        { 
           var url = BuildReadUrl(base64Id);
           var response = SendRequest(url);
           return response.ToFormattedResult(_dev, base64Id, url, ApiOperation.Read);
        }
        public object WriteValue(string base64Id, string value)
        {
            string url = BuildWriteUrl(base64Id,value);
            var response = SendRequest(url);
            return response.ToFormattedResult(_dev, base64Id, url, ApiOperation.Write);        }
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
        internal string BuildReadUrl(string base64Id)
        {
            return $"{_baseUrl}Read&OA={base64Id}&PIN={_pin}";
        }
        internal string BuildWriteUrl(string base64Id , string value)
        {
            return $"{_baseUrl}Write&OA={base64Id};{value}&PIN={_pin}";
        }
    }   

   public class ApiResponse
    {
        public bool IsSuccess { get; set; }
        public int StatusCode { get; set; }
        public string Content { get; set; } = string.Empty;
        public string ErrorMessage { get; set; } = string.Empty;
        internal object ToFormattedResult(bool devMode,string base64Id,string  apiCall, ApiOperation operation)
        {
            if (devMode)
            {
                return new 
                    {
                        IsSuccess = this.IsSuccess,
                        StatusCode = this.StatusCode,
                        Content = this.Content,
                        ErrorMessage = this.ErrorMessage,
                        PointId = base64Id,
                        APICaall = apiCall,
                        Op = operation.ToString()
                    };              
            }
            else
            {
               if (!string.IsNullOrEmpty(this.ErrorMessage))
                {
                    return "Error: " + this.ErrorMessage;
                }
                if (operation == ApiOperation.Write)
                {
                    return this.IsSuccess ? "Success" : "Write Failed";
                }
                else 
                {
                    return this.Content;
                }
            }
        }
    }

   public enum ApiOperation { Read, Write }
}