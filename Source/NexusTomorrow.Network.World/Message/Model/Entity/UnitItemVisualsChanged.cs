using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Entity;

[WireMessage(GameMessageOpcode.Server0933, Direction.ServerToClient)]
public sealed partial class UnitItemVisualsChanged
{
    [UInt(32)] public uint Unknown0 { get; set; }
    [Array(CountBits = 32)] public List<ItemVisual> Unknown1 { get; set; } = [];
}
