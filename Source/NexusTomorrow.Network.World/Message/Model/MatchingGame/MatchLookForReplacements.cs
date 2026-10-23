using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.MatchingGame;

[WireMessage(GameMessageOpcode.Client05D5, Direction.ClientToServer)]
public sealed partial class MatchLookForReplacements
{
    [UInt(32)] public uint Unknown0 { get; private set; }
}
