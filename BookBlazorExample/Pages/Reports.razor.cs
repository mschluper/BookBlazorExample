using BookBlazorExample.Models;
using BookBlazorExample.Services;
using Microsoft.AspNetCore.Components;

namespace BookBlazorExample.Pages
{
    public partial class Reports
    {
        private List<Requirement> requirements = new();
        private DateTime lastModifiedDate = DateTime.MinValue;

        [Inject]
        public IRequirementService? RequirementService { get; set; }

        private void SetChildrenLevel(Requirement req)
        {
            req.Children.ForEach(c => 
            { 
                c.Level = 1 + req.Level; 
                SetChildrenLevel(c); 
            });
        }

        protected override async Task OnInitializedAsync()
        {
            (var reqs, lastModifiedDate) = await RequirementService!.GetRequirements();

            var reqLookup = reqs.ToDictionary(r => r.Id, r => r);
            foreach(Requirement req in reqs)
            {
                if (req.ParentId != null)
                {
                    reqLookup[req.ParentId].Children.Add(req);
                }
            }
            var roots = reqs.Where(r => r.ParentId == null).ToList();
            roots.ForEach(root =>
            {
                SetChildrenLevel(root);
            });
            var maxLevel = reqs.Max(r => r.Level);
            for (var level = maxLevel; level >= 0; level--)
            {
                reqs.Where(r => r.Level == level)
                    .ToList()
                    .ForEach(r =>
                    {
                        r.DescendantCount = r.Children.Sum(c => 1 + c.DescendantCount);
                        r.Definition = r.DescendantCount > 0 ? $"Aggregate of {r.DescendantCount}" : r.Definition;
                        r.PassedTests = r.DescendantCount > 0 ? !r.Children.Any(c => !c.PassedTests) : r.PassedTests;
                    });
            }

            requirements = reqs;
        }
    }
}