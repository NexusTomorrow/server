using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Utility;

[WireMessage(GameMessageOpcode.ClientInspectPlayerRequest, Direction.ClientToServer)]
public sealed partial class ClientInspectPlayerRequest
{
    [UInt(32)] public uint UnitId { get; set; }
}
