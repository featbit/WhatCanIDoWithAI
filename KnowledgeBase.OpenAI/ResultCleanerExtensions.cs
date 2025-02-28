namespace KnowledgeBase.OpenAI
{
    public static class ResultCleanerExtensions
    {
        public static string CleanResult(this string result)
        {
            int startIndex = result.IndexOf('{');
            int endIndex = result.LastIndexOf('}');
            if (startIndex == -1 || endIndex == -1 || startIndex >= endIndex)
            {
                return string.Empty;
            }
            return result.Substring(startIndex, endIndex - startIndex + 1);
        }
    }
}
