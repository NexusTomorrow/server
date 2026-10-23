using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared.Message;

namespace NexusTomorrow.Network.World.Message.Model.Entity;

/// <summary>
/// A unit says a line.
/// </summary>
/// <remarks>
/// The client shows the line in a speech bubble and posts it on the NPCSay channel, or posts it on the Emote channel alone.
/// </remarks>
[WireMessage(GameMessageOpcode.Server0753, Direction.ServerToClient)]
[Verified(Rung.Purpose)]
public sealed partial class UnitSay
{
    /// <summary>
    /// The line.
    /// </summary>
    public Server0738Part0 Text { get; set; } = new();

    /// <summary>
    /// The unit speaking, by unit id.
    /// </summary>
    [UInt(32)] public uint UnitId { get; set; }

    [UInt(1)] public bool Unknown2 { get; set; }
    [UInt(1)] public bool Unknown3 { get; set; }

    /// <summary>
    /// How far the line carries.
    /// </summary>
    [Float32] public float Range { get; set; }
}
