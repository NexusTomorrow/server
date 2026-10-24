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

    [Float32] public float Unknown1 { get; set; }
}
