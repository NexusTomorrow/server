using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

[Message(GameMessageOpcode.ServerTitleUpdate)]
[WireMessage(GameMessageOpcode.ServerTitleUpdate, Direction.ServerToClient)]
public sealed partial class ServerTitleUpdate : IWritable
{
    [UInt(14)] public ushort TitleId { get; set; }
    [UInt(1)] public bool Alreadyowned { get; set; }
    [UInt(1)] public bool Revoked { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(TitleId, 14u);
        writer.Write(Alreadyowned);
        writer.Write(Revoked);
    }
}
