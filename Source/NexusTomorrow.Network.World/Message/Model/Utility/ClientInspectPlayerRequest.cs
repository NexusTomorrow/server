using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Utility;

[Message(GameMessageOpcode.ClientInspectPlayerRequest)]
[WireMessage(GameMessageOpcode.ClientInspectPlayerRequest, Direction.ClientToServer)]
public sealed partial class ClientInspectPlayerRequest : IReadable
{
    [UInt(32)] public uint UnitId { get; set; }

    public void Read(GamePacketReader reader)
    {
        UnitId = reader.ReadUInt();
    }
}
