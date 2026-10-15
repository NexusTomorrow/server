using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Housing;

[Message(GameMessageOpcode.ServerHousingAttachUnitToDecorItem)]
[WireMessage(GameMessageOpcode.ServerHousingAttachUnitToDecorItem, Direction.ServerToClient)]
public sealed partial class ServerHousingAttachUnitToDecorItem : IWritable
{
    public Identity ResidenceIdentity { get; set; }
    [UInt(64)] public ulong DecorId { get; set; }
    [UInt(64)] public ulong AttachedUnitId { get; set; }

    public void Write(GamePacketWriter writer)
    {
        ResidenceIdentity.Write(writer);
        writer.Write(DecorId);
        writer.Write(AttachedUnitId);
    }
}
