using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Friendship;

[WireMessage(GameMessageOpcode.Server03AE, Direction.ServerToClient)]
public sealed partial class FriendshipAccountPrivateNote
{
    [UInt(64)] public ulong Unknown0 { get; set; }
    [WideString] public string Unknown1 { get; set; } = "";
}
