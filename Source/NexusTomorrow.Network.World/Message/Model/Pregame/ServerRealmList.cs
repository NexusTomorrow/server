using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NetworkMessage = NexusTomorrow.Network.Message.Model.Shared.Message;

namespace NexusTomorrow.Network.World.Message.Model.Pregame;

// Client only processes message when on RealmSelect screen.
[WireMessage(GameMessageOpcode.ServerRealmList, Direction.ServerToClient)]
public sealed partial class ServerRealmList
{
    [UInt(64)] public ulong Unused { get; set; }
    [Array(CountBits = 32)] public List<RealmInfo> Realms { get; set; } = [];
    [Array(CountBits = 32)] public List<NetworkMessage> Messages { get; set; } = [];
}
