using SFT.Core.Domain;

namespace SFT.Core.Commands;

public interface IFactoryTrackerCommands
{
    Task<Mine> AddMineAsync(Mine mine, CancellationToken cancellationToken = default);
    Task UpdateMineAsync(Mine mine, CancellationToken cancellationToken = default);
    Task DeleteMineAsync(int mineId, CancellationToken cancellationToken = default);

    Task<MineOutput> AddMineOutputAsync(MineOutput output, CancellationToken cancellationToken = default);
    Task UpdateMineOutputAsync(MineOutput output, CancellationToken cancellationToken = default);
    Task DeleteMineOutputAsync(int outputId, CancellationToken cancellationToken = default);

    Task<MiningStation> AddMiningStationAsync(MiningStation station, CancellationToken cancellationToken = default);
    Task UpdateMiningStationAsync(MiningStation station, CancellationToken cancellationToken = default);
    Task DeleteMiningStationAsync(int stationId, CancellationToken cancellationToken = default);

    Task<Factory> AddFactoryAsync(Factory factory, CancellationToken cancellationToken = default);
    Task UpdateFactoryAsync(Factory factory, CancellationToken cancellationToken = default);
    Task DeleteFactoryAsync(int factoryId, CancellationToken cancellationToken = default);

    Task<FactoryLevel> AddFactoryLevelAsync(FactoryLevel level, CancellationToken cancellationToken = default);
    Task UpdateFactoryLevelAsync(FactoryLevel level, CancellationToken cancellationToken = default);
    Task DeleteFactoryLevelAsync(int levelId, CancellationToken cancellationToken = default);
    Task MoveLevelUpAsync(int levelId, CancellationToken cancellationToken = default);
    Task MoveLevelDownAsync(int levelId, CancellationToken cancellationToken = default);

    Task<FactoryOutput> AddFactoryOutputAsync(FactoryOutput output, CancellationToken cancellationToken = default);
    Task UpdateFactoryOutputAsync(FactoryOutput output, CancellationToken cancellationToken = default);
    Task DeleteFactoryOutputAsync(int outputId, CancellationToken cancellationToken = default);

    Task<FactoryInput> AddFactoryInputAsync(FactoryInput input, CancellationToken cancellationToken = default);
    Task UpdateFactoryInputAsync(FactoryInput input, CancellationToken cancellationToken = default);
    Task DeleteFactoryInputAsync(int inputId, CancellationToken cancellationToken = default);
}
