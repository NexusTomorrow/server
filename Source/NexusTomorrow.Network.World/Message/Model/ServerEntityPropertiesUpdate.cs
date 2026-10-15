using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model;

[WireMessage(GameMessageOpcode.ServerEntityPropertiesUpdate, Direction.ServerToClient)]
public sealed partial class ServerEntityPropertiesUpdate
{
    [UInt(32)] public uint UnitId { get; set; }
    [Array(CountBits = 8)] public List<PropertyValue> Properties { get; set; } = [];
}
