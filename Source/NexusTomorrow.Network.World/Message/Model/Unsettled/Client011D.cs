using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Unsettled;

[WireMessage(GameMessageOpcode.Client011D, Direction.ClientToServer)]
public sealed partial class Client011D
{
    [UInt(32)] public uint Unknown0 { get; set; }
}
