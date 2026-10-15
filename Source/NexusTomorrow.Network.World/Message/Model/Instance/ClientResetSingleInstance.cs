using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Instance;

[WireMessage(GameMessageOpcode.ClientResetSingleInstance, Direction.ClientToServer)]
public sealed partial class ClientResetSingleInstance
{
    [UInt(32)] public uint InstancePortalUnitId { get; private set; }
}
