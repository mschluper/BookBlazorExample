namespace RequirementsTracer
{
    // Don't try to automatically verify state transitions. Verification is still a human activity.
    // Do refer to State Property Names.
    public class Requirement
    {
        public string Id { get; set; } = string.Empty;

        public string? ParentId { get; set; }

        public string Definition { get; set; } = string.Empty;

        public bool IsCoveredByTest { get; set; }

        public bool PassedTests { get; set; }
    }
}
