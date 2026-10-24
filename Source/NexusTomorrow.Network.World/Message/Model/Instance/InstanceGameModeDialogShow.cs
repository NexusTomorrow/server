using NexusTomorrow.Game.Static.Setting;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Instance;

/// <summary>
/// Opens the instance settings window for a portal.
/// </summary>
/// <remarks>
/// The client keeps the portal's id, starts interacting with the portal when its InstancePortal row is of type 1, and raises
/// ShowInstanceGameModeDialog with the other fields.
/// </remarks>
[WireMessage(GameMessageOpcode.Server0164, Direction.ServerToClient)]
[Verified(Rung.Purpose)]
public sealed partial class InstanceGameModeDialogShow
{
    /// <summary>
    /// The portal, by unit id. <see cref="InstanceSettingsClosed"/>, <see cref="InstanceResetRequest"/> and
    /// <see cref="InstanceSettingsSet"/> send it back.
    /// </summary>
    [UInt(32)] public uint PortalUnitId { get; set; }

    /// <summary>
    /// The difficulties the portal offers, as a mask of 1 shifted by a <see cref="WorldDifficulty"/>: bit 0 Normal, bit 1
    /// Veteran.
    /// </summary>
    [UInt(2)] public byte AllowedDifficulties { get; set; }

    /// <summary>
    /// Bit 0: level scaling is allowed. Bit 9: the instance has prime levels. The client reads no other bit.
    /// </summary>
    [UInt(11)] public ushort Flags { get; set; }

    /// <summary>
    /// The difficulty of the player's existing instance, or <see cref="WorldDifficulty.Count"/> when there is none.
    /// </summary>
    [UInt(2)] public WorldDifficulty ExistingDifficulty { get; set; }

    /// <summary>
    /// The maximum prime level for the world, which add-ons read as nMaxPrimeLevelWorld.
    /// </summary>
    [UInt(8)] public byte MaxPrimeLevelWorld { get; set; }

    /// <summary>
    /// The maximum prime level for the group, which add-ons read as nMaxPrimeLevelGroup.
    /// </summary>
    [UInt(8)] public byte MaxPrimeLevelGroup { get; set; }

    [UInt(8)] public byte Unknown6 { get; set; }

    /// <summary>
    /// Whether the existing instance has level scaling.
    /// </summary>
    [UInt(1)] public bool ExistingScaling { get; set; }

    /// <summary>
    /// The existing instance's prime level.
    /// </summary>
    [UInt(8)] public byte ExistingPrimeLevel { get; set; }

    /// <summary>
    /// Whether the instance is a raid.
    /// </summary>
    [UInt(1)] public bool IsRaid { get; set; }

    /// <summary>
    /// Whether a prime level may be chosen.
    /// </summary>
    [UInt(1)] public bool PrimeAllowed { get; set; }
}
