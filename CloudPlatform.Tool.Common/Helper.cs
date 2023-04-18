namespace CloudPlatform.Tool.Common
{
    public static class Helper
    {
        public static string FormatCopyright2Html(string copyrightCode)
        {
            if (string.IsNullOrWhiteSpace(copyrightCode))
            {
                return copyrightCode;
            }

            var result = copyrightCode.Replace("[c]", "&copy;")
                .Replace("[year]", DateTime.UtcNow.Year.ToString());
            return result;
        }
    }
}