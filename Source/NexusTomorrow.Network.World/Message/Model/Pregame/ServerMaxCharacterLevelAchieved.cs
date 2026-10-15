using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Pregame;

[Message(GameMessageOpcode.ServerMaxCharacterLevelAchieved)]
[WireMessage(GameMessageOpcode.ServerMaxCharacterLevelAchieved, Direction.ServerToClient)]
public sealed partial class ServerMaxCharacterLevelAchieved : IWritable
{
    [UInt(32)] public uint Level { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(Level);
    }
}
