using System;
using System.Text.Json;

namespace ArticyProjectJsonParser.Core
{
    public class Parser{
        public ParsingResult Parse(string pathToJsonFile)
        {
            var jDoc = JsonDocument.Parse(pathToJsonFile);

            return new ParsingResult(jDoc);
        }
    }
}