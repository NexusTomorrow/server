using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Static;

namespace NexusTomorrow.Network.World.Message.Model.Pregame;

[Message(GameMessageOpcode.ServerCharacterSelectFail)]
[WireMessage(GameMessageOpcode.ServerCharacterSelectFail, Direction.ServerToClient)]
public sealed partial class ServerCharacterSelectFail : IWritable
{
    [UInt(3)] public CharacterSelectResult Result { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(Result, 3u);
    }
}
