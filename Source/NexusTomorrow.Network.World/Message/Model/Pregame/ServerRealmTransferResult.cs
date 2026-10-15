using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Static;

namespace NexusTomorrow.Network.World.Message.Model.Pregame;

// Client only processes message when on CharacterSelect screen.
[Message(GameMessageOpcode.ServerRealmTransferResult)]
public class ServerRealmTransferResult : IWritable
{
    public CharacterModifyResult Result { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(Result, 6u);
    }
}
