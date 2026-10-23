using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Friendship;

[WireMessage(GameMessageOpcode.Server03AD, Direction.ServerToClient)]
public sealed partial class FriendshipAccountPublicNote
{
    [UInt(32)] public uint Unknown0 { get; set; }
    [WideString] public string Unknown1 { get; set; } = "";
}
