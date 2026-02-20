using SFT.Core.Domain;

namespace SFT.Core.Queries;

public interface IFactoryTrackerQueries
{
    Task<IReadOnlyList<Mine>> GetMinesAsync(CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Factory>> GetFactoriesAsync(CancellationToken cancellationToken = default);
}
