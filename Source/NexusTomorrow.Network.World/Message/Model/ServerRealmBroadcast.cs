using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model;

// Can be sent to player in Game, CharacterSelect, or RealmSelect screens
[Message(GameMessageOpcode.ServerRealmBroadcast)]
[WireMessage(GameMessageOpcode.ServerRealmBroadcast, Direction.ServerToClient)]
public sealed partial class ServerRealmBroadcast : IWritable
{
    [UInt(2)] public BroadcastTier Tier { get; set; }
    [WideString] public string Message { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(Tier, 2);
        writer.WriteStringWide(Message);
    }
}
