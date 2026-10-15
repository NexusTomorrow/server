using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

[Message(GameMessageOpcode.ServerRealmEncrypted)]
[WireMessage(GameMessageOpcode.ServerRealmEncrypted, Direction.ServerToClient)]
public sealed partial class ServerRealmEncrypted : IWritable
{
    [Blob(32, LengthIncludesSelf = true)] public byte[] Data { get; set; } = [];

    public void Write(GamePacketWriter writer)
    {
        writer.Write(Data.Length + 4);
        writer.WriteBytes(Data);
    }
}
