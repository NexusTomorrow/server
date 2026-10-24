using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Hazard;

/// <summary>
/// Sets the state of hazards. The client copies each record into the one its hazard id keys, creating it when none exists, then raises
/// HazardUpdated for its add-ons. HazardsLib builds each hazard it lists from these records.
/// </summary>
[WireMessage(GameMessageOpcode.Message010A, Direction.ServerToClient)]
[Verified(Rung.Purpose)]
public sealed partial class HazardStates
{
    /// <summary>
    /// The hazard states to copy in. Each replaces the state the client holds for its hazard id.
    /// </summary>
    [VerifiedMember, Array(CountBits = 32)] public List<HazardState> States { get; set; } = [];
}
