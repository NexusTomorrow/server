using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model;

[WireMessage(GameMessageOpcode.ServerEntityVisualUpdate, Direction.ServerToClient)]
public sealed partial class ServerEntityVisualUpdate
{
    [UInt(32)] public uint UnitId { get; set; }
    [UInt(5)] public byte Race { get; set; }
    [UInt(2)] public byte Sex { get; set; }
    [UInt(18)] public uint CreatureId { get; set; }
    [UInt(17)] public uint DisplayInfo { get; set; }
    [UInt(15)] public ushort OutfitInfo { get; set; }
    [UInt(32)] public uint ItemColorSetId { get; set; }
    [UInt(1)] public bool Unknown6 { get; set; }

    [Array(CountBits = 32)] public List<ItemVisual> ItemVisuals { get; set; } = [];
}
