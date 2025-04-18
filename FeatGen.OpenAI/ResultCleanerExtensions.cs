﻿namespace FeatGen.OpenAI
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
        public static string CleanJsCodeQuote(this string result)
        {
            result = result.Trim();
            if(result.StartsWith("```javascript"))
                result = result.Replace("```javascript", "");
            if (result.EndsWith("```"))
                result = result.Substring(0, result.Length - 3);
            return result;
        }
        public static string CleanMarkdownCodeQuote(this string result)
        {
            result = result.Trim();
            if (result.StartsWith("```md"))
                result = result.Replace("```md", "");
            if (result.EndsWith("```"))
                result = result.Substring(0, result.Length - 3);
            return result;
        }
        public static string CleanJsonCodeQuote(this string result)
        {
            result = result.Trim();
            if (result.StartsWith("```json"))
                result = result.Replace("```json", "");
            if (result.EndsWith("```"))
                result = result.Substring(0, result.Length - 3);
            return result;
        }

        public static string CleanJsonCodeQuoteV2(this string result)
        {
            result = result.Trim();
            int pos = result.IndexOf("```json");
            if (pos >= 0 && result.Contains("```json") && pos < 200)
                result = result.Substring(pos+7);
            if (result.EndsWith("```"))
                result = result.Substring(0, result.Length - 3);
            return result;
        }
        public static string CleanCssCodeQuote(this string result)
        {
            result = result.Trim();
            if (result.StartsWith("```css"))
                result = result.Replace("```css", "");
            if (result.EndsWith("```"))
                result = result.Substring(0, result.Length - 3);
            return result;
        }

        public static string RemoveDQBetweenDQ(this string result)
        {
            var element = result.IndexOf("\"");
            return "";
        }
    }
}
