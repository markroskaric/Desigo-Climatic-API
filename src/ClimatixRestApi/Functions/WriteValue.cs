namespace ClimatixRestApi
{
    public static class WriteValueExtensions
    {
        public static object WriteValue(this Connection conn, string base64Id, string value)
        {
            string url = BuildWriteUrl(conn, base64Id, value);
            ApiResponse response = conn.SendRequest(url);
            return response.ToFormattedResult(conn._dev, url, ApiOperation.Write);
        }
        internal static string BuildReadUrl(Connection conn, string base64Id)
        {
            return $"{conn._baseUrl}Read&OA={base64Id}&PIN={conn._pin}";
        }
        internal static string BuildWriteUrl(Connection conn, string base64Id, string value)
        {
            return $"{conn._baseUrl}Write&OA={base64Id};{value}&PIN={conn._pin}";
        }
    }

}