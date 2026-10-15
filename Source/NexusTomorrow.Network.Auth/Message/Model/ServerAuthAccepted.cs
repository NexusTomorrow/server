using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.Auth.Message.Model;

[Message(GameMessageOpcode.ServerAuthAccepted)]
[WireMessage(GameMessageOpcode.ServerAuthAccepted, Direction.ServerToClient)]
public sealed partial class ServerAuthAccepted : IWritable
{
    [UInt(32)] public uint DisconnectedForLag { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(DisconnectedForLag);
    }
}
