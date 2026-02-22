using SFT.Core.Domain;

namespace SFT.Core.Queries;

public interface IFactoryTrackerQueries
{
    Task<IReadOnlyList<Mine>> GetMinesAsync(CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Factory>> GetFactoriesAsync(CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Resource>> GetResourcesAsync(CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Resource>> GetRawResourcesAsync(CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Resource>> GetResourcesProducibleFromAsync(int rawResourceId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<ResourceRecipeView>> GetResourceRecipesAsync(CancellationToken cancellationToken = default);
}
