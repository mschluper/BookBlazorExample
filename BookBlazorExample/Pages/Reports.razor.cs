using BookBlazorExample.Models;
using BookBlazorExample.Services;
using BookBlazorExample.States;
using Microsoft.AspNetCore.Components;

namespace BookBlazorExample.Pages
{
    public partial class Reports
    {
        private ReportState state = new();

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
            var propertyDictionary = await RequirementService!.GetPropertiesByState();
            state.PropertyDefinitions = propertyDictionary.Keys.SelectMany(stateName =>
            {
                var list = propertyDictionary[stateName];
                var shortName = stateName.Substring(25);    // "BookBlazorExample.States.".Length equals 25
                return list.Select(e => new StateProperty
                {
                    Name = $"{shortName}.{e.Name}",
                    Definition = e.Definition,
                });
            }).ToList();

            (var reqs, state.LastModifiedDate) = await RequirementService!.GetRequirements();

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

            state.Requirements = reqs;
        }

        private void OnChangeProperty(ChangeEventArgs e)
        {
            var propertyName = e.Value?.ToString();
            state.SelectedProperty = state.PropertyDefinitions.FirstOrDefault(pd => pd.Name == propertyName);
        }
    }
}