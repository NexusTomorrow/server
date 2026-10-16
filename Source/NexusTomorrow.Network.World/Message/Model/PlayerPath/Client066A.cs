using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.PlayerPath;

[WireMessage(GameMessageOpcode.Client066A, Direction.ClientToServer)]
public sealed partial class Client066A
{
    [FixedArray(4), UInt(32)] public uint[] Unknown0 { get; private set; } = new uint[4];
}
