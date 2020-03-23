using System.Text.Json;

namespace ArticyProjectJsonParser.Core
{
    public class ParsingResult
    {
        public JsonDocument Result { get; set; }
        public ParsingResult(JsonDocument jDoc)
        {
            Result = jDoc;
        }
    }
}