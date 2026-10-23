using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.MatchingGame;

[WireMessage(GameMessageOpcode.Client05B5, Direction.ClientToServer)]
public sealed partial class MatchingQueueLeave
{
    [UInt(5)] public byte Unknown0 { get; private set; }
}
