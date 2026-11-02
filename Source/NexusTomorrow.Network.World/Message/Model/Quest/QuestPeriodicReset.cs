using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Quest;

/// <summary>
/// The client's trigger for its QuestPeriodicReset event.
/// </summary>
/// <remarks>
/// Only when <see cref="ResetToken"/> differs from the value the client holds does it act: it stores the value and raises
/// QuestPeriodicReset with <see cref="ResetFlags"/>'s four bits.
/// </remarks>
[WireMessage(GameMessageOpcode.Server0362, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class QuestPeriodicReset
{
    /// <summary>
    /// The periodic reset token; the client acts only when it differs from the token it last held.
    /// </summary>
    [VerifiedMember, UInt(64)] public ulong ResetToken { get; set; }

    /// <summary>
    /// The four reset flags, one bit each, raised as the QuestPeriodicReset event's four booleans.
    /// </summary>
    [VerifiedMember, UInt(4)] public byte ResetFlags { get; set; }
}
