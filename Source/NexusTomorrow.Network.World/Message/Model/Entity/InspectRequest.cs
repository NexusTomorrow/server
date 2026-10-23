using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Entity;

[WireMessage(GameMessageOpcode.Client0552, Direction.ClientToServer)]
public sealed partial class InspectRequest
{
    [UInt(32)] public uint Unknown0 { get; set; }
}
