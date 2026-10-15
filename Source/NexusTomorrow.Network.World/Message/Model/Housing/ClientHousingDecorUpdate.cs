using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Static;

namespace NexusTomorrow.Network.World.Message.Model.Housing;

[WireMessage(GameMessageOpcode.ClientHousingDecorUpdate, Direction.ClientToServer)]
public sealed partial class ClientHousingDecorUpdate
{
    [UInt(3)] public DecorUpdateOperation Operation { get; private set; }
    [UInt(32), Count] public uint DecorUpdateCount { get; private set; }
    [Array(CountFrom = nameof(DecorUpdateCount))] public List<DecorUpdate> DecorUpdates { get; private set; } = [];

    // Every update's service token bit follows the last update: element i belongs to DecorUpdates[i].
    [Array(CountFrom = nameof(DecorUpdateCount)), UInt(1)] public List<bool> UseServiceTokens { get; private set; } = [];
}
