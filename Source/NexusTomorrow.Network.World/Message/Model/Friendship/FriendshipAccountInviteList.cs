using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Friendship;

[WireMessage(GameMessageOpcode.Server039F, Direction.ServerToClient)]
public sealed partial class FriendshipAccountInviteList
{
    [WireStruct]
    public sealed partial class FriendshipAccountInvite
    {
        [UInt(64)] public ulong Unknown0 { get; set; }
        [UInt(2)] public byte Unknown1 { get; set; }
        [WideString] public string Unknown2 { get; set; } = "";
        [WideString] public string Unknown3 { get; set; } = "";
        [Float32] public float Unknown4 { get; set; }
        [UInt(32)] public uint Unknown5 { get; set; }
    }

    [UInt(32)] public uint Unknown0 { get; set; }
    [Array(CountBits = 32)] public List<FriendshipAccountInvite> Unknown1 { get; set; } = [];
}
