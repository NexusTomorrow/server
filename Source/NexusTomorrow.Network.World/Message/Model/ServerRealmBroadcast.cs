using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model;

// Can be sent to player in Game, CharacterSelect, or RealmSelect screens
[WireMessage(GameMessageOpcode.ServerRealmBroadcast, Direction.ServerToClient)]
public sealed partial class ServerRealmBroadcast
{
    [UInt(2)] public BroadcastTier Tier { get; set; }
    [WideString] public string Message { get; set; }
}
