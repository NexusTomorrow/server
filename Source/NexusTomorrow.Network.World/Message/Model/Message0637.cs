using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Entity;

namespace NexusTomorrow.Network.World.Message.Model;

[WireMessage(GameMessageOpcode.Message0637, Direction.ClientToServer)]
public sealed partial class Message0637
{
    [UInt(32)] public uint Unknown0 { get; set; }
    [Array(CountBits = 32)] public List<Server0262Part5> Unknown1 { get; } = [];
}
