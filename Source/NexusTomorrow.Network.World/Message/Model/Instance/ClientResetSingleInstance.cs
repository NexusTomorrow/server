using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Instance;

[Message(GameMessageOpcode.ClientResetSingleInstance)]
[WireMessage(GameMessageOpcode.ClientResetSingleInstance, Direction.ClientToServer)]
public sealed partial class ClientResetSingleInstance : IReadable
{
    [UInt(32)] public uint InstancePortalUnitId { get; private set; }

    public void Read(GamePacketReader reader)
    {
        InstancePortalUnitId = reader.ReadUInt();
    }
}
