using BookBlazorExample.Models;

namespace BookBlazorExample.Services
{
    public interface IRequirementService
    {
        Task<(List<Requirement>, DateTime)> GetRequirements();
    }

    public class RequirementService : IRequirementService
    {
        public async Task<(List<Models.Requirement>, DateTime)> GetRequirements()
        {
            try
            {
                var fileName = $"{System.IO.Directory.GetCurrentDirectory()}/requirementCoverage.json";
                string coverageData = await System.IO.File.ReadAllTextAsync(fileName);
                var list = System.Text.Json.JsonSerializer.Deserialize<List<Models.Requirement>>(coverageData) ?? new();
                DateTime lastWriteTime = File.GetLastWriteTime(fileName);
                return (list, lastWriteTime);
            }
            catch (Exception)
            {
                return (new(), DateTime.MinValue);
            }
        }
    }
}
