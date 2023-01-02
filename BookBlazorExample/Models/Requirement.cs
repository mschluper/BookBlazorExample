namespace BookBlazorExample.Models
{
    public class Requirement
    {
        public string Id { get; set; } = string.Empty;

        public string? ParentId { get; set; }

        public string Definition { get; set; } = string.Empty;

        public bool IsCoveredByTest { get; set; }

        public bool PassedTests { get; set; }

        public List<Requirement> Children { get; set; } = new();


        public int Level { get; set; } = 0;
        public int DescendantCount { get; set; } = 0;
    }
}
