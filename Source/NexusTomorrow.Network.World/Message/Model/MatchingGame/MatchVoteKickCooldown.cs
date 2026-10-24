using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.MatchingGame;

/// <summary>
/// Has the client print a time in the matching text, which it does only for two values of <see cref="Unknown0"/>.
/// </summary>
[WireMessage(GameMessageOpcode.Server0616, Direction.ServerToClient)]
[Verified(Rung.Purpose)]
public sealed partial class MatchVoteKickCooldown
{
    [UInt(6)] public byte Unknown0 { get; set; }
    [UInt(32)] public uint Unknown1 { get; set; }
}
