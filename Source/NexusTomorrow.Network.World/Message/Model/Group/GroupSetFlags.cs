using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Group;

/// <summary>
/// Sets the group's flags: the request GroupLib.ConvertToRaid sends, among others.
/// </summary>
/// <remarks>
/// GroupLib.ConvertToRaid sends the group's flags with 0x2 set; GroupLib.SetJoinRequestMethod and GroupLib.SetReferralMethod send
/// them with their pair of bits set from a GroupLib.InvitationMethod.
/// </remarks>
[WireMessage(GameMessageOpcode.Client0406, Direction.ClientToServer)]
[Verified(Rung.Fields)]
public sealed partial class GroupSetFlags
{
    /// <summary>
    /// The player's current group, by id.
    /// </summary>
    [UInt(64)] public ulong GroupId { get; set; }

    /// <summary>
    /// The group's flags as the client asks for them, by bit as <see cref="GroupFlagsChanged.Flags"/>.
    /// </summary>
    [UInt(32)] public uint Flags { get; set; }
}
