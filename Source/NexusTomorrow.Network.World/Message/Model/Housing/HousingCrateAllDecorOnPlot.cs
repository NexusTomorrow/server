using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Housing;

/// <summary>
/// Crates the decor on one of a residence's plots.
/// </summary>
[WireMessage(GameMessageOpcode.Server051F, Direction.ServerToClient)]
[Verified(Rung.Purpose)]
public sealed partial class HousingCrateAllDecorOnPlot
{
    /// <summary>
    /// The residence, by id.
    /// </summary>
    public RealmScopedId ResidenceId { get; set; } = new();

    [UInt(32)] public uint Unknown1 { get; set; }
}
