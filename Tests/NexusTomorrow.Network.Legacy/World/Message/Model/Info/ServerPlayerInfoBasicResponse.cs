using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Info;

[Message(GameMessageOpcode.ServerPlayerInfoBasicResponse)]
public class ServerPlayerInfoBasicResponse : IWritable
{
    public PlayerInfoBase BaseData { get; set; }

    public void Write(GamePacketWriter writer)
    {
        BaseData.Write(writer);
    }
}
