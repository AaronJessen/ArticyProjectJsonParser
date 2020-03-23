using System.ComponentModel.DataAnnotations;

namespace ArticyProjectJsonParser.Database.Models
{
    public class ModelDbo
    {
        public string Type { get; set; } 
        public string TechnicalName { get; set; } 
        [Key] public string Id { get; set; } 
        public string Parent { get; set; } 
        public string DisplayName { get; set; } 
        public string Color { get; set; } 
        public string Text { get; set; } 
        public string ExternalId { get; set; } 
        public long ShortId { get; set; } 
    }
}