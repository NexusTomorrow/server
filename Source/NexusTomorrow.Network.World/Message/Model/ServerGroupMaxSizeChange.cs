using NexusTomorrow.Game.Static.Group;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

[WireMessage(GameMessageOpcode.ServerGroupMaxSizeChange, Direction.ServerToClient)]
public sealed partial class ServerGroupMaxSizeChange
{
    [UInt(64)] public ulong GroupId { get; set; }

    [UInt(32)] public GroupFlags NewFlags { get; set; }

    [UInt(32)] public uint NewMaxSize { get; set; }
}
