using NexusTomorrow.Game.Static.Prerequisite;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Map;

/// <summary>
/// Sets the player's planes.
/// </summary>
[WireMessage(GameMessageOpcode.Server0172, Direction.ServerToClient)]
[Verified(Rung.Purpose)]
public sealed partial class PlayerPlanesChanged
{
    /// <summary>
    /// The client gives this value no meaning. It stores it but never reads it.
    /// </summary>
    [UInt(8)] public uint Unknown0 { get; set; }

    /// <summary>
    /// The player's planes, which <see cref="PrerequisiteType.Plane"/> tests with the requirement's value as a mask.
    /// </summary>
    [UInt(8)] public uint Planes { get; set; }
}
