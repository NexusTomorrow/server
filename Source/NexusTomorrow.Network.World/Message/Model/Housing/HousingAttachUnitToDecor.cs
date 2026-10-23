using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Housing;

/// <summary>
/// Attaches a unit to a piece of a residence's decor.
/// </summary>
[WireMessage(GameMessageOpcode.Server053A, Direction.ServerToClient)]
[Verified(Rung.Purpose)]
public sealed partial class HousingAttachUnitToDecor
{
    /// <summary>
    /// The residence, by id.
    /// </summary>
    public RealmScopedId ResidenceId { get; set; } = new();

    [UInt(64)] public ulong Unknown1 { get; set; }
    [UInt(32)] public uint Unknown2 { get; set; }
}
