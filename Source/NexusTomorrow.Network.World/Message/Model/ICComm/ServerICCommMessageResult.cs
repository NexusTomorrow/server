using NexusTomorrow.Game.Static.ICComm;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.ICComm;

[Message(GameMessageOpcode.ServerICCommMessageResult)]
public class ServerICCommMessageResult : IWritable
{
    public ulong IccomId { get; set; }
    public uint MessageId { get; set; }
    public ICCommMessageResult Result { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(IccomId);
        writer.Write(MessageId);
        writer.Write(Result, 4u);
    }
}
