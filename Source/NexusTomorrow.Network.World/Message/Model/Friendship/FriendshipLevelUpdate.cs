using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Friendship;

[WireMessage(GameMessageOpcode.Server03C6, Direction.ServerToClient)]
public sealed partial class FriendshipLevelUpdate
{
    public RealmScopedId Unknown0 { get; set; } = new();
    [UInt(32)] public uint Unknown1 { get; set; }
}
