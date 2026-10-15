using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

[WireMessage(GameMessageOpcode.ClientStorefrontRequestCatalog, Direction.ClientToServer)]
public sealed partial class ClientStorefrontRequestCatalog
{
    [UInt(14)] public ushort Unknown0 { get; private set; }
}
