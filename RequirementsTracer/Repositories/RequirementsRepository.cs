
namespace RequirementsTracer.Repositories
{
    public class RequirementRepository
    {
        public async Task<List<Requirement>> GetRequirements()
        {
            List<Requirement> requirements = new();
            await Task.Run(() =>
            {
                requirements = new()
                {
                    new ()
                    {
                        Id = "ProductList",
                        ParentId = null,
                    },
                    new ()
                    {
                        Id = "DisplayProducts",
                        ParentId = "ProductList",
                        Definition = "",
                        PassedTests = false,
                    },
                };
            });
            return requirements;
        }
    }
}
