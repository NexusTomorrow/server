using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Guild;

/// <summary>
/// Reports a war plot match's results for both sides.
/// </summary>
/// <remarks>
/// The client raises WarPartyMatchResults with each side's rating, repair cost, destroyed plugs and war coins earned.
/// </remarks>
[WireMessage(GameMessageOpcode.Server0954, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class WarPartyMatchResults
{
    /// <summary>
    /// Each side's results.
    /// </summary>
    [FixedArray(2)] public WarPartyMatchResult[] Sides { get; set; } = [new(), new()];
}
