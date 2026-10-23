using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.MatchingGame;

[WireMessage(GameMessageOpcode.Client05C8, Direction.ClientToServer)]
public sealed partial class MatchReadyResponse
{
    [UInt(1)] public bool Unknown0 { get; private set; }
}
