using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Static;

namespace NexusTomorrow.Network.World.Message.Model.Pregame;

[Message(GameMessageOpcode.ServerCharacterCreate)]
public class ServerCharacterCreate : IWritable
{
    public ulong CharacterId { get; set; }
    public uint WorldId { get; set; }
    public CharacterModifyResult Result { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(CharacterId);
        writer.Write(WorldId);
        writer.Write(Result, 3);
    }
}
