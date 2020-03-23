using System.ComponentModel.DataAnnotations;

namespace ArticyProjectJsonParser.Database.Models
{
    public class PinDbo
    {
        public string Type { get; set; } 
        public string Text { get; set; } 
        [Key] public string Id { get; set; } 
        public string Owner { get; set; } 
    }
}