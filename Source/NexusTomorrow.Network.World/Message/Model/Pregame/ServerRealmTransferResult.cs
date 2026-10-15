using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Static;

namespace NexusTomorrow.Network.World.Message.Model.Pregame;

// Client only processes message when on CharacterSelect screen.
[Message(GameMessageOpcode.ServerRealmTransferResult)]
[WireMessage(GameMessageOpcode.ServerRealmTransferResult, Direction.ServerToClient)]
public sealed partial class ServerRealmTransferResult : IWritable
{
    [UInt(6)] public CharacterModifyResult Result { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(Result, 6u);
    }
}
