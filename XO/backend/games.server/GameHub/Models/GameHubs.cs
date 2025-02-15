using System.ComponentModel.DataAnnotations;

namespace GameHub.Models
{
    public class GameHubs
    {
        public Guid Id { get; set; }
        [Required(ErrorMessage = "Creator is required")]
        public string Creator { get; set; }
        public string? PlayerX { get; set; } // Первый игрок (создатель)
        public string? PlayerO { get; set; } // Второй игрок
        public DateTime CreatedAt { get; set; }
        public string Status { get; set; }
    }
}