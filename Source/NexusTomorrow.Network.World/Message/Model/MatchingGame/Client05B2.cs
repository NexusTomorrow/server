using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.MatchingGame;

[WireMessage(GameMessageOpcode.Client05B2, Direction.ClientToServer)]
public sealed partial class Client05B2
{
    [UInt(5)] public byte Unknown0 { get; private set; }
    [UInt(32)] public uint Unknown1 { get; private set; }
    [UInt(1)] public bool Unknown2 { get; private set; }
}
