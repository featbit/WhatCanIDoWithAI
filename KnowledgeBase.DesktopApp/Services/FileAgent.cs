namespace FeatGen.CodingAgent
{
    public class FileAgent
    {
        public static void AppendFileContent(string filePath, string newText)
        {
            System.IO.File.Create(filePath).Dispose();
            System.IO.File.AppendAllText(filePath, newText);
        }

        public static void RewriteFileContent(string filePath, string newText)
        {
            System.IO.File.WriteAllText(filePath, newText);
        }

        public static void CreateAndInitFile(string filePath, string newText)
        {
            System.IO.File.WriteAllText(filePath, newText);
        }

        public static async Task CreateAndInitFileAsync(string filePath, string newText)
        {
            await System.IO.File.WriteAllTextAsync(filePath, newText);
        }

        public static async Task ReplaceFileTextAsync(string filePath, string textToReplace, string newText)
        {
            string text = await System.IO.File.ReadAllTextAsync(filePath);
            text = text.Replace(textToReplace, newText);
            await System.IO.File.WriteAllTextAsync(filePath, text);
        }
    }
}
