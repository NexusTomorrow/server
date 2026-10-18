using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.MatchingGame;

[WireMessage(GameMessageOpcode.Server0628, Direction.ServerToClient)]
public sealed partial class Server0628
{
    [UInt(5)] public byte Unknown0 { get; set; }
    [UInt(32)] public uint Unknown1 { get; set; }
}
