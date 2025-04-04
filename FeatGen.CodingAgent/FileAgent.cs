﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public static void CreateFolder(string folderPath)
        {
            if (!System.IO.Directory.Exists(folderPath))
                System.IO.Directory.CreateDirectory(folderPath);
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

        public static string ReadFileContent(string filePath)
        {
            return System.IO.File.ReadAllText(filePath);
        }


        public static async Task CreateAndUpsertFolderAndFileAsync(string folderPath, string fileName, string newText, bool replaceOldText)
        {
            CreateFolder(folderPath);
            string filePath = folderPath + "/" + fileName;
            bool fileExisits = System.IO.File.Exists(filePath);
            if (!fileExisits)
            {
                using (var fs = System.IO.File.Create(filePath))
                {
                }
            }
            if (fileExisits && replaceOldText == false)
                return;
            await System.IO.File.WriteAllTextAsync(filePath, newText);
        }
    }
}
