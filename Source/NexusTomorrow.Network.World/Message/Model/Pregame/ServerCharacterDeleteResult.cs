using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Static;

namespace NexusTomorrow.Network.World.Message.Model.Pregame;

[Message(GameMessageOpcode.ServerCharacterDeleteResult)]
public class ServerCharacterDeleteResult : IWritable
{
    public CharacterModifyResult Result { get; set; }
    public uint Data { get; set; } // used to select type of guild to show in error when result = DeleteFailed_GuildMaster

    public void Write(GamePacketWriter writer)
    {
        writer.Write(Result, 6u);
        writer.Write(Data);
    }
}
