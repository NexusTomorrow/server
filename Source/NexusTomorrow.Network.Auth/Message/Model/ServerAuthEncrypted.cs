using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.Auth.Message.Model;

[Message(GameMessageOpcode.ServerAuthEncrypted)]
[WireMessage(GameMessageOpcode.ServerAuthEncrypted, Direction.ServerToClient)]
public sealed partial class ServerAuthEncrypted : IWritable
{
    [Blob(32, LengthIncludesSelf = true)] public byte[] Data { get; set; } = [];

    public void Write(GamePacketWriter writer)
    {
        writer.Write(Data.Length + 4);
        writer.WriteBytes(Data);
    }
}
