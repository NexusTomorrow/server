using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

[WireMessage(GameMessageOpcode.Server0252, Direction.ServerToClient)]
public sealed partial class Server0252
{
    [FixedArray(3), UInt(32)] public uint[] Unknown0 { get; set; } = new uint[3];
}
