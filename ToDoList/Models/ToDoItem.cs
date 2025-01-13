namespace ToDoList.Models
{
    public class ToDoItem
    {
        public int ToDoItemId { get; set; }
        public string Title { get; set; }
        public bool IsCompleted { get; set; }
        public int CategoryId { get; set; } // Foreign key to Category

        public Category Category { get; set; } // Navigation property

        public DateTime DueDate { get; set; } // Дата за задачата
        public string Notes { get; set; } // Бележки

    }
}
