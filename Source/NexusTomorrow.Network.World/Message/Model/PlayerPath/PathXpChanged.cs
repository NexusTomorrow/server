using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.PlayerPath;

/// <summary>
/// Sets the XP of the player's active path (<see cref="PathState.ActivePath"/>).
/// </summary>
/// <remarks>
/// The client raises UpdatePathXp with the gain over the XP it held, and PathLevelUp when the level the new XP gives is higher.
/// </remarks>
[WireMessage(GameMessageOpcode.Server01AA, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class PathXpChanged
{
    /// <summary>
    /// The active path's XP, which PlayerPathLib.GetPathXP returns for that path.
    /// </summary>
    [UInt(32)] public uint Xp { get; set; }
}
