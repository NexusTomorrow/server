using NexusTomorrow.Game.Static.Group;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

[WireMessage(GameMessageOpcode.ClientGroupMarkUnit, Direction.ClientToServer)]
public sealed partial class ClientGroupMark
{
    [UInt(32)] public GroupMarker Marker { get; set; }

    [UInt(32)] public uint UnitId { get; set; }
}
