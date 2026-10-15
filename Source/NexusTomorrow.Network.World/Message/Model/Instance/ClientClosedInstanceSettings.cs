using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Instance;

[Message(GameMessageOpcode.ClientClosedInstanceSettings)]
[WireMessage(GameMessageOpcode.ClientClosedInstanceSettings, Direction.ClientToServer)]
public sealed partial class ClientClosedInstanceSettings : IReadable
{
    [UInt(32)] public uint InstancePortalUnitId { get; private set; }

    public void Read(GamePacketReader reader)
    {
        InstancePortalUnitId = reader.ReadUInt();
    }
}
