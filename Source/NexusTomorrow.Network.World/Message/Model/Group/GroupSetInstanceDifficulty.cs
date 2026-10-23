using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Group;

/// <summary>
/// Sets the group's instance difficulty: the request GroupLib.SetInstanceDifficulty sends.
/// </summary>
[WireMessage(GameMessageOpcode.Client0412, Direction.ClientToServer)]
[Verified(Rung.Fields)]
public sealed partial class GroupSetInstanceDifficulty
{
    /// <summary>
    /// The player's current group, by id.
    /// </summary>
    [UInt(64)] public ulong GroupId { get; set; }

    /// <summary>
    /// The difficulty asked for, a GroupLib.Difficulty value: 0 normal, 1 veteran.
    /// </summary>
    [UInt(2)] public byte Difficulty { get; set; }
}
