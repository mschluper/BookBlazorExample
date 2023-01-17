using BookBlazorExample.Models;

namespace BookBlazorExample.Services
{
    /// <summary>
    /// Service that supports reporting on requirements and their coverage
    /// </summary>
    public interface IRequirementService
    {
        /// <summary>
        /// Get all requirements from a generated JSON file and the date and time this file was generated
        /// </summary>
        /// <returns>A list of requirements</returns>
        Task<(List<Requirement>, DateTime)> GetRequirements();

        /// <summary>
        /// Get property definitions of all states
        /// </summary>
        /// <returns>A list (one for each state) of property definitions</returns>
        Task<Dictionary<string, List<StateProperty>>> GetPropertiesByState();
    }

    /// <inheritdoc/>
    public class RequirementService : IRequirementService
    {
        /// <inheritdoc/>
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

        /// <inheritdoc/>
        public async Task<Dictionary<string, List<StateProperty>>> GetPropertiesByState()
        {
            try
            {
                var fileName = $"{System.IO.Directory.GetCurrentDirectory()}/statePropertyDefinitions.json";
                string propertyDefinitions = await System.IO.File.ReadAllTextAsync(fileName);
                var dictionary = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, List<StateProperty>>>(propertyDefinitions) ?? new();
                return dictionary;
            }
            catch (Exception)
            {
                return new();
            }
        }
    }
}
