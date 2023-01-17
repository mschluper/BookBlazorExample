using BookBlazorExample.Models;

namespace BookBlazorExample.States
{
    public class ReportState
    {
        public StateProperty? SelectedProperty { get; set; }

        public List<Requirement> Requirements = new();

        public List<StateProperty> PropertyDefinitions = new();

        public DateTime LastModifiedDate = DateTime.MinValue;
    }
}
