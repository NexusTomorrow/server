using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Crafting;

[WireMessage(GameMessageOpcode.Client085B, Direction.ClientToServer)]
public sealed partial class Client085B
{
    [UInt(64)] public ulong Unknown0 { get; private set; }
    [Array(CountBits = 32), UInt(32)] public uint[] Unknown1 { get; private set; }
}
