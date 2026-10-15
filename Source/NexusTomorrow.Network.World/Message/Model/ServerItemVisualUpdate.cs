using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model;

[WireMessage(GameMessageOpcode.ServerItemVisualUpdate, Direction.ServerToClient)]
public sealed partial class ServerItemVisualUpdate
{
    [UInt(32)] public uint Guid { get; set; }
    [Array(CountBits = 32)] public List<ItemVisual> ItemVisuals { get; set; } = [];
}
