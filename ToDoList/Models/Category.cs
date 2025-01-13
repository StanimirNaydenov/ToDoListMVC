using System.ComponentModel.DataAnnotations;

namespace ToDoList.Models
{
    public class Category
    {
        public int CategoryId { get; set; }
        [Required(ErrorMessage = "The Name field is required.")]
        [StringLength(100, ErrorMessage = "The Name cannot exceed 100 characters.")]
        public string Name { get; set; }
        public int UserId { get; set; } // Foreign key to User

        public User User { get; set; } // Navigation property
        public ICollection<ToDoItem> ToDoItems { get; set; } // Navigation property

    }
}
