namespace CQRS_example_project.Domain
{
    public class ToDo
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public bool IsCompleted { get; set; }

    }
}
