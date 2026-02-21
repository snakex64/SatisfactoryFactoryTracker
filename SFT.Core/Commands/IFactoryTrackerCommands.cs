using SFT.Core.Domain;

namespace SFT.Core.Commands;

public interface IFactoryTrackerCommands
{
    Task<Mine> AddMineAsync(Mine mine, CancellationToken cancellationToken = default);
    Task UpdateMineAsync(Mine mine, CancellationToken cancellationToken = default);
    Task DeleteMineAsync(int mineId, CancellationToken cancellationToken = default);

    Task<Factory> AddFactoryAsync(Factory factory, CancellationToken cancellationToken = default);
    Task UpdateFactoryAsync(Factory factory, CancellationToken cancellationToken = default);
    Task DeleteFactoryAsync(int factoryId, CancellationToken cancellationToken = default);

    Task<FactoryLevel> AddFactoryLevelAsync(FactoryLevel level, CancellationToken cancellationToken = default);
    Task UpdateFactoryLevelAsync(FactoryLevel level, CancellationToken cancellationToken = default);
    Task DeleteFactoryLevelAsync(int levelId, CancellationToken cancellationToken = default);
}
