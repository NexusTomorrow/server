using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Friendship;

[WireMessage(GameMessageOpcode.Server03B2, Direction.ServerToClient)]
public sealed partial class FriendshipAccountCharacterLevel
{
    [UInt(32)] public uint Unknown0 { get; set; }
    public RealmScopedId Unknown1 { get; set; } = new();
    [UInt(8)] public uint Unknown2 { get; set; }
}
