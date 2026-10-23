using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Entity;

/// <summary>
/// Plays an emote on another unit.
/// </summary>
[WireMessage(GameMessageOpcode.Server037F, Direction.ServerToClient)]
[Verified(Rung.Purpose)]
public sealed partial class UnitEmote
{
    /// <summary>
    /// The emote, an Emotes table id; 0 ends the one playing.
    /// </summary>
    [UInt(14)] public ushort EmoteId { get; set; }

    /// <summary>
    /// The value that picks the emote's animation.
    /// </summary>
    [UInt(32)] public uint Seed { get; set; }

    /// <summary>
    /// The unit emoting, by unit id.
    /// </summary>
    [UInt(32)] public uint UnitId { get; set; }

    /// <summary>
    /// The emote's target, by unit id.
    /// </summary>
    [UInt(32)] public uint TargetUnitId { get; set; }

    /// <summary>
    /// True when the emote has a target, which picks its targeted animation and text.
    /// </summary>
    [UInt(1)] public bool HasTarget { get; set; }

    [UInt(1)] public bool Unknown5 { get; set; }
}
