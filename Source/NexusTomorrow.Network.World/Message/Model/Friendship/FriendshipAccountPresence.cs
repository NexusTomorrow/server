using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Friendship;

[WireMessage(GameMessageOpcode.Server03AA, Direction.ServerToClient)]
public sealed partial class FriendshipAccountPresence
{
    [UInt(32)] public uint Unknown0 { get; set; }
    [UInt(3)] public byte Unknown1 { get; set; }
}
