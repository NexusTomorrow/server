using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Housing;

/// <summary>
/// A neighbor, as <see cref="HousingNeighbors"/> lists one and <see cref="HousingNeighborUpdate"/> changes one.
/// </summary>
[WireStruct]
public sealed partial class HousingNeighbor
{
    [UInt(64)] public ulong Unknown0 { get; set; }

    /// <summary>
    /// The client gives this value no meaning.
    /// </summary>
    [UInt(64)] public ulong Unknown1 { get; set; }

    /// <summary>
    /// The neighbor, by character id, which HousingLib.NeighborEvict and the other neighbor calls send back.
    /// </summary>
    public RealmScopedId CharacterId { get; set; } = new();

    /// <summary>
    /// A HousingLib.NeighborPermissionLevel value: HousingLib.GetNeighborList's ePermissionNeighbor.
    /// </summary>
    [UInt(32)] public uint Permission { get; set; }
}
