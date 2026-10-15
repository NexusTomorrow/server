using NexusTomorrow.Game.Static.Group;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model;

[WireMessage(GameMessageOpcode.ServerGroupActionResult, Direction.ServerToClient)]
public sealed partial class ServerGroupActionResult
{
    [UInt(64)] public ulong GroupId { get; set; }
    public Identity Identity { get; set; }
    [UInt(32)] public GroupActionResult Result { get; set; }
}
