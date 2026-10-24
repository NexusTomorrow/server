using System.Numerics;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Map;

/// <summary>
/// Sets the destination marker that <see cref="DestinationMarkerCleared"/> clears.
/// </summary>
[WireMessage(GameMessageOpcode.Server07E5, Direction.ServerToClient)]
[Verified(Rung.Purpose)]
public sealed partial class DestinationMarkerSet
{
    /// <summary>
    /// The marker's position.
    /// </summary>
    [Float32] public Vector3 Position { get; set; }

    /// <summary>
    /// The marker shows while the player is within this distance of <see cref="Position"/> horizontally and within 10 vertically;
    /// farther away, an arrow points to it.
    /// </summary>
    [Float32] public float Radius { get; set; }
}
