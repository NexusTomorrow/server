using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Friendship;

[WireMessage(GameMessageOpcode.Server0399, Direction.ServerToClient)]
public sealed partial class FriendshipAccountUpdate
{
    [UInt(32)] public uint Unknown0 { get; set; }
    [UInt(3)] public byte Unknown1 { get; set; }
    [Array(CountBits = 32)] public List<FriendshipAccountCharacter> Unknown2 { get; set; } = [];
}
