using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Friendship;

[WireMessage(GameMessageOpcode.Client03B3, Direction.ClientToServer)]
public sealed partial class FriendshipAddByIdentity
{
    public RealmScopedId Unknown0 { get; set; } = new();
    [UInt(4)] public byte Unknown1 { get; set; }
    [WideString] public string Unknown2 { get; set; } = "";
}
