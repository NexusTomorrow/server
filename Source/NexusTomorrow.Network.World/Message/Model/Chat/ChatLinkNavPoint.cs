using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Message.Model.Chat;

/// <summary>
/// A link of kind 10: a point on the map.
/// </summary>
[WireStruct]
[UnionCase(10)]
public sealed partial class ChatLinkNavPoint : IChatLinkPayload
{
    /// <summary>
    /// The point's zone, a MapZone table id.
    /// </summary>
    [UInt(14)] public ushort MapZoneId { get; set; }

    /// <summary>
    /// The point's first coordinate.
    /// </summary>
    [Float32] public float X { get; set; }

    /// <summary>
    /// The point's second coordinate.
    /// </summary>
    [Float32] public float Y { get; set; }
}
