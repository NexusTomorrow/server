using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Entity;

[WireMessage(GameMessageOpcode.Server093A, Direction.ServerToClient)]
public sealed partial class UnitPropertiesChanged
{
    [UInt(32)] public uint Unknown0 { get; set; }
    [Array(CountBits = 8)] public List<UnitPropertyValue> Unknown1 { get; set; } = [];
}
