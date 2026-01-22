
namespace Domain
{
    public class WorkItem
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public int Order { get; set; }
        public int ProjectColumnId { get; set; }
        public string? AssignedToUserId { get; set; }
        public string AuthorUserId { get; set; }
        public Priority Priority { get; set; } = Priority.Medium;
        public DateOnly? DueDate { get; set; }
        public ItemType Type { get; set; } = ItemType.Task;
    }

    public enum ItemType
    {
        Task = 1,
        Bug = 2,
        Feature = 3
    }
    public enum Priority
    {
        Lowest = 1,
        Low = 2,
        Medium = 3,
        High = 4,
        Highest = 5
    }
}
