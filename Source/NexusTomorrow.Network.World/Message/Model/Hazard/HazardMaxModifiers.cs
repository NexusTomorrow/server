using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Hazard;

/// <summary>
/// Sets the modifiers the client applies to each hazard's maximum. The client works the maximum out itself from a hazard's
/// <see cref="HazardState.MeterMax"/> and these entries, and every multiplier it holds starts at 1 and every offset at 0.
/// </summary>
[WireMessage(GameMessageOpcode.Message010B, Direction.ServerToClient)]
[Verified(Rung.Purpose)]
public sealed partial class HazardMaxModifiers
{
    /// <summary>
    /// The modifier for each hazard, by position: entry N is hazard N's. The client sizes its copy to the last id in the Hazard table
    /// plus one and takes the list as sent, so the list must not be longer than that.
    /// </summary>
    [VerifiedMember, Array(CountBits = 32)] public List<HazardMaxModifier> HazardModifiers { get; set; } = [];

    [Array(CountBits = 32)] public List<HazardMaxModifier> Unknown1 { get; set; } = [];
}
