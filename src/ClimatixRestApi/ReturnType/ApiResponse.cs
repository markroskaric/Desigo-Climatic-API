namespace ClimatixRestApi
{
    public class ApiResponse
    {
        public bool IsSuccess { get; set; }
        public int StatusCode { get; set; }
        public string Content { get; set; } = string.Empty;
        public string ErrorMessage { get; set; } = string.Empty;
        internal object ToFormattedResult(bool devMode, string apiCall, ApiOperation operation)
        {
            if (devMode)
            {
                return new
                {
                    IsSuccess = this.IsSuccess,
                    StatusCode = this.StatusCode,
                    Content = this.Content,
                    ErrorMessage = this.ErrorMessage,
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

}