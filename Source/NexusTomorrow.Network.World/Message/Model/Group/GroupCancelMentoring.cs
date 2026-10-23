using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Group;

/// <summary>
/// Asks to stop mentoring: the request GroupLib.CancelMentoring sends.
/// </summary>
[WireMessage(GameMessageOpcode.Client043B, Direction.ClientToServer)]
[Verified(Rung.Fields)]
public sealed partial class GroupCancelMentoring
{
    /// <summary>
    /// The player's current group, by id.
    /// </summary>
    [UInt(64)] public ulong GroupId { get; set; }
}
