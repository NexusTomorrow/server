using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Cinematic;

/// <summary>
/// Reports the cinematic camera's subject as a unit, when a <see cref="CinematicCameraTransition"/> runs.
/// </summary>
[WireMessage(GameMessageOpcode.Client071F, Direction.ClientToServer)]
[Verified(Rung.Fields)]
public sealed partial class CinematicCameraSubjectUnit
{
    /// <summary>
    /// The unit the cinematic camera looks at, by unit id.
    /// </summary>
    [UInt(32)] public uint UnitId { get; private set; }
}
