using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.MatchingGame;

[WireMessage(GameMessageOpcode.Client05B6, Direction.ClientToServer)]
public sealed partial class Client05B6
{
    [UInt(5)] public byte Unknown0 { get; private set; }
}
