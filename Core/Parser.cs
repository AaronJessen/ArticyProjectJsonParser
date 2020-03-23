using System;
using System.IO;
using System.Text.Json;

namespace ArticyProjectJsonParser.Core
{
    public class Parser
    {
        public ParsingResult Parse(string pathToJsonFile)
        {
            var json = File.ReadAllText(pathToJsonFile);

            var jDoc = JsonDocument.Parse(json);

            return new ParsingResult(jDoc);
        }
    }
}