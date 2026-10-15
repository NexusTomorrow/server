using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

[Message(GameMessageOpcode.ServerLogout)]
[WireMessage(GameMessageOpcode.ServerLogout, Direction.ServerToClient)]
public sealed partial class ServerLogout : IWritable
{
    [UInt(1)] public bool Requested { get; set; }
    [UInt(5)] public LogoutReason Reason { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(Requested);
        writer.Write(Reason, 5);
    }
}
