using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Group;

/// <summary>
/// Adds a member to one of the player's groups.
/// </summary>
/// <remarks>
/// For its current group the client raises Group_Add with the member's name.
/// </remarks>
[WireMessage(GameMessageOpcode.Server0405, Direction.ServerToClient)]
[Verified(Rung.Purpose)]
public sealed partial class GroupAdd
{
    /// <summary>
    /// The group, by id.
    /// </summary>
    [UInt(64)] public ulong GroupId { get; set; }

    [UInt(32)] public uint Unknown1 { get; set; }

    /// <summary>
    /// The member.
    /// </summary>
    public Server0405Part0 Member { get; set; } = new();
}
