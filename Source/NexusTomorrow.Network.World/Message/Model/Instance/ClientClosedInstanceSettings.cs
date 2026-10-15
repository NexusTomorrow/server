using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Instance;

[WireMessage(GameMessageOpcode.ClientClosedInstanceSettings, Direction.ClientToServer)]
public sealed partial class ClientClosedInstanceSettings
{
    [UInt(32)] public uint InstancePortalUnitId { get; private set; }
}
