using NexusTomorrow.Game.Static.Group;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

[WireMessage(GameMessageOpcode.ServerGroupMarkUnit, Direction.ServerToClient)]
public sealed partial class ServerGroupMarkUnit
{
    [UInt(64)] public ulong GroupId { get; set; }

    [UInt(32)] public GroupMarker Marker { get; set; }

    [UInt(32)] public uint UnitId { get; set; }
}
