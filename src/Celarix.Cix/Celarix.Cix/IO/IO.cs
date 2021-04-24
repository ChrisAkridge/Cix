using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Celarix.Cix.Compiler.IO.Models;
using NLog;

namespace Celarix.Cix.Compiler.IO
{
    internal static class IO
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        
        public static IList<Line> SplitFileIntoLines(string filePath)
        {
            string fileName = Path.GetFileName(filePath);
            logger.Trace($"Splitting {fileName} into lines...");
            string fileText = File.ReadAllText(filePath);
            
            // https://stackoverflow.com/questions/1547476/easiest-way-to-split-a-string-on-newlines-in-net
            var lineStrings = fileText.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
            var lines = new List<Line>();

            var lineStringIndex = 0;
            var startingCharacterIndex = 0;
            foreach (var lineString in lineStrings)
            {
                lines.Add(new Line
                {
                    Text = lineString,
                    SourceFilePath = filePath,
                    FileLineNumber = lineStringIndex,
                    FileStartCharacterIndex = startingCharacterIndex
                });

                lineStringIndex += 1;
                startingCharacterIndex += lineString.Length;
            }

            logger.Trace($"Split {filePath} into {lines.Count} lines");
            return lines;
        }

        public static string JoinLinesIntoString(IEnumerable<Line> lines)
        {
            var builder = new StringBuilder();

            foreach (var line in lines)
            {
                builder.AppendLine(line.Text);
            }

            return builder.ToString();
        }

        public static string GetSaveTempsPath(string outputFilePath, SaveTempsFile saveTempsFile) =>
            Path.GetFileNameWithoutExtension(outputFilePath)
            + saveTempsFile switch
            {
                SaveTempsFile.Preprocessed => "_preprocessed.cix",
                SaveTempsFile.AbstractSyntaxTree => "_ast.json",
                _ => throw new ArgumentException($"Unknown --save-temps file type {saveTempsFile}.",
                    nameof(saveTempsFile))
            };
    }
}
