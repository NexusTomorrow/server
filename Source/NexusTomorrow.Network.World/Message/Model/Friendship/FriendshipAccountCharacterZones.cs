using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Friendship;

[WireMessage(GameMessageOpcode.Server03A4, Direction.ServerToClient)]
public sealed partial class FriendshipAccountCharacterZones
{
    [WireStruct]
    public sealed partial class Server03A4Part0
    {
        [UInt(64)] public ulong Unknown0 { get; set; }
        public RealmScopedId Unknown1 { get; set; } = new();
        [UInt(15)] public uint Unknown2 { get; set; }
    }

    [Array(CountBits = 16)] public List<Server03A4Part0> Unknown0 { get; set; } = [];
}
