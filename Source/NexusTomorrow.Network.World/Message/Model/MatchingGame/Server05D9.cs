using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.MatchingGame;

[WireMessage(GameMessageOpcode.Server05D9, Direction.ServerToClient)]
public sealed partial class Server05D9
{
    [FixedArray(16), UInt(32)] public uint[] Unknown0 { get; set; } = new uint[16];
}
