using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Static;

namespace NexusTomorrow.Network.World.Message.Model.Info;

[Message(GameMessageOpcode.ServerGenericError)]
public class ServerGenericError : IWritable
{
    public GenericError Error { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(Error, 8u);
    }
}
