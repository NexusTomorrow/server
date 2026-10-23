using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Housing;

/// <summary>
/// Updates the build on one of a residence's plots.
/// </summary>
/// <remarks>
/// When a build starts, the client raises HousingBuildStarted with the plot's index plus one.
/// </remarks>
[WireMessage(GameMessageOpcode.Server053B, Direction.ServerToClient)]
[Verified(Rung.Purpose)]
public sealed partial class HousingBuildState
{
    /// <summary>
    /// The residence, by id.
    /// </summary>
    public RealmScopedId ResidenceId { get; set; } = new();

    [UInt(32)] public uint Unknown1 { get; set; }
    [UInt(32)] public uint Unknown2 { get; set; }
    [UInt(3)] public byte Unknown3 { get; set; }
}
