using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Housing;

/// <summary>
/// Crates the decor on one of a residence's plots.
/// </summary>
[WireMessage(GameMessageOpcode.Server051F, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class HousingCrateAllDecorOnPlot
{
    /// <summary>
    /// The residence, by id.
    /// </summary>
    [VerifiedMember] public RealmScopedId ResidenceId { get; set; } = new();

    /// <summary>
    /// The crate operation's flags.
    /// </summary>
    [VerifiedMember, UInt(32)] public uint Flags { get; set; }
}
