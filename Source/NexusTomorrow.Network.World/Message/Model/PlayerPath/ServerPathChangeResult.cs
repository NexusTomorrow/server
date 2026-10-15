using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Static;

namespace NexusTomorrow.Network.World.Message.Model.PlayerPath;


[Message(GameMessageOpcode.ServerPathChangeResult)]
public class ServerPathChangeResult : IWritable
{
    public GenericError Result { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(Result, 8);
    }
}
