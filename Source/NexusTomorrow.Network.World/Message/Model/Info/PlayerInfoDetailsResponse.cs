using NexusTomorrow.Game.Static.Entity;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using Path = NexusTomorrow.Game.Static.PlayerPath.Path;

namespace NexusTomorrow.Network.World.Message.Model.Info;

/// <summary>
/// Answers <see cref="PlayerInfoRequest"/> with a player's name and faction, and their path, class, level and last-online time.
/// </summary>
/// <remarks>
/// The client takes it as a <see cref="PlayerInfoResponse"/> first, which answers every name lookup for the player, then hands the
/// details to every detail request waiting on the player.
/// </remarks>
[WireMessage(GameMessageOpcode.Server0599, Direction.ServerToClient)]
[Verified(Rung.Purpose)]
public sealed partial class PlayerInfoDetailsResponse
{
    /// <summary>
    /// The player's id, name and faction.
    /// </summary>
    public PlayerInfoSummary Summary { get; set; } = new();

    /// <summary>
    /// Whether <see cref="Path"/> and <see cref="ClassId"/> are given: FriendshipLib and HousingLib.GetNeighborList pass them on to add-ons
    /// only while it is set.
    /// </summary>
    [UInt(1)] public bool HasClassAndPath { get; set; }

    /// <summary>
    /// The player's path, which add-ons read as nPathId.
    /// </summary>
    [UInt(3)] public Path Path { get; set; }

    /// <summary>
    /// The player's class id, a <see cref="Class"/> value, which add-ons read as nClassId.
    /// </summary>
    [UInt(14)] public ushort ClassId { get; set; }

    /// <summary>
    /// The player's level, which add-ons read as nLevel.
    /// </summary>
    [UInt(32)] public uint Level { get; set; }

    /// <summary>
    /// Whether the player is offline: FriendshipLib gives add-ons a last-online time only while it is set.
    /// </summary>
    [UInt(1)] public bool IsOffline { get; set; }

    /// <summary>
    /// When the player was last online, in days relative to now: a negative value is that many days ago, and zero or more means none.
    /// </summary>
    [Float32] public float LastOnlineDays { get; set; }
}
