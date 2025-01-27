using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArticleCollector.utils
{
    public class PuppyAgentSBlockBuilder
    {
        public static async Task Build()
        {
            var articlesFolderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "articles");
            var mdFiles = Directory.GetFiles(articlesFolderPath, "*.md");

            foreach (var file in mdFiles)
            {
                string content = await File.ReadAllTextAsync(file);
                Console.WriteLine(content); // Process the content as needed
            }
        }
    }
}
