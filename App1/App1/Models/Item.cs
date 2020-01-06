using System.ComponentModel.DataAnnotations;

namespace App1.Models
{
    public class Item
    {
        [Key]
        public string? Id { get; set; }

        public string? Text { get; set; }

        public string? Description { get; set; }
    }
}
