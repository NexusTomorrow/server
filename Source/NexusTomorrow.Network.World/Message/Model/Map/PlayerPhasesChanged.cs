using NexusTomorrow.Game.Static.Prerequisite;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Map;

/// <summary>
/// Sets the player's phases and the phases the player can see.
/// </summary>
[WireMessage(GameMessageOpcode.Server0171, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class PlayerPhasesChanged
{
    /// <summary>
    /// The player's phase mask: world locations, map regions and other group members' visibility are matched against it.
    /// </summary>
    [UInt(32)] public uint Phases { get; set; }

    /// <summary>
    /// The phases the player can see: other group members' phase masks are matched against it, and
    /// <see cref="PrerequisiteType.InPhase"/> and <see cref="PrerequisiteType.CanSeePhase"/> test one bit of it, the requirement's value
    /// giving the bit's index.
    /// </summary>
    [UInt(32)] public uint VisiblePhases { get; set; }
}
