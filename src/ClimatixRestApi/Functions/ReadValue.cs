namespace ClimatixRestApi
{
    public static class ReadValueExtensions
    {
        public static object ReadValue(this Connection conn, string[] oa, string[]? ioa = null)
            => conn.ReadValueInternal(oa, ioa);

        private static object ReadValueInternal(this Connection conn, IEnumerable<string> oa, IEnumerable<string>? ioa)
        {
            string url = BuildReadUrl(conn, oa, ioa);
            ApiResponse response = conn.SendRequest(url);
            return response.ToFormattedResult(conn._dev, url, ApiOperation.Read);
        }

        internal static string BuildReadUrl(Connection conn, IEnumerable<string> oa, IEnumerable<string>? ioa)
        {
            if (ioa == null && oa != null)
            {
                return $"{conn._baseUrl}Read&OA={string.Join(",", oa)}&PIN={conn._pin}";
            }
            else if (ioa != null && oa != null)
            {
                return $"{conn._baseUrl}Read&OA={string.Join(",", oa)}&IOA={string.Join(",", ioa)}&PIN={conn._pin}";
            }
            else
            {
                throw new ArgumentException("At least one identifier is required.", $"{nameof(ioa)} or {nameof(oa)}");
            }
        }
    }
}