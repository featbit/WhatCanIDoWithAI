using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArticleCollector.utils
{
    public class PuppyAgentSBlockBuilder
    {
        public static async Task<IEnumerable<string>> Build()
        {
            var articlesFolderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "articles");
            var mdFiles = Directory.GetFiles(articlesFolderPath, "*.md");

            var contents = new List<string>();
            foreach (var file in mdFiles)
            {
                string content = await File.ReadAllTextAsync(file);
                contents.Add(content);
            }
            return contents;
        }

        public static async Task<string> RecomposeBlock(VertexGemini geminiAgent, string blockContent)
        {
            var result = await geminiAgent.Generate($"Generate some questions would ask for block content: {blockContent}");
            return result;
        }
    }
}
