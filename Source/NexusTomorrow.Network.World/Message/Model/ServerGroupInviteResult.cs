using NexusTomorrow.Game.Static.Group;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

[WireMessage(GameMessageOpcode.ServerGroupInviteResult, Direction.ServerToClient)]
public sealed partial class ServerGroupInviteResult
{
    [UInt(64)] public ulong GroupId { get; set; }
    [WideString] public string Name { get; set; }
    [UInt(5)] public GroupInviteResult Result { get; set; }
}
