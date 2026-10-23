using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Housing;

/// <summary>
/// Sets the residence's options: the request Residence:SetGroundClutterHidden, Residence:SetSkyplotsHidden,
/// Residence:SetNeighborHarvestSplit and Residence:SetNeighborGardenSplit send.
/// </summary>
/// <remarks>
/// Each sends all four fields as the client holds them, with only its own option changed.
/// </remarks>
[WireMessage(GameMessageOpcode.Client050E, Direction.ClientToServer)]
[Verified(Rung.Fields)]
public sealed partial class HousingSetResidenceOptions
{
    /// <summary>
    /// The player's current residence.
    /// </summary>
    public RealmScopedId ResidenceId { get; } = new();

    /// <summary>
    /// The residence's flags as the client asks for them: bit 0 hides the ground clutter, bit 1 the skyplots.
    /// </summary>
    [UInt(32)] public uint Flags { get; private set; }

    /// <summary>
    /// How the residence's harvest is split with neighbors: Residence:SetNeighborHarvestSplit's argument, below 6.
    /// </summary>
    [UInt(3)] public byte HarvestSplit { get; private set; }

    /// <summary>
    /// How the residence's garden is split with neighbors: Residence:SetNeighborGardenSplit's argument, below 6.
    /// </summary>
    [UInt(3)] public byte GardenSplit { get; private set; }
}
