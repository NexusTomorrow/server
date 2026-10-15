using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model;

[WireMessage(GameMessageOpcode.ServerGroupJoin, Direction.ServerToClient)]
public sealed partial class ServerGroupJoin
{
    public Identity TargetPlayer { get; set; } = new();
    public Group Group { get; set; } = new Group();
}
