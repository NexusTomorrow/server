using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Group;

[WireMessage(GameMessageOpcode.Server0427, Direction.ServerToClient)]
public sealed partial class GroupJoined
{
    public RealmScopedId Unknown0 { get; set; } = new();
    public GroupRecord Unknown1 { get; set; } = new GroupRecord();
}
