using System.ComponentModel.DataAnnotations;

namespace ToDoList.Models
{
    public class User
    {
        public int UserId { get; set; }
        [Required]
        public string Username { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string PasswordHash { get; set; }

        public ICollection<Category> Categories { get; set; } // Navigation property
    }
}
