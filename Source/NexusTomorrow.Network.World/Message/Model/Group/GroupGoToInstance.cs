using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Group;

/// <summary>
/// Asks to go to the group's instance: the request GroupLib.GotoGroupInstance sends.
/// </summary>
[WireMessage(GameMessageOpcode.Client0411, Direction.ClientToServer)]
[Verified(Rung.Fields)]
public sealed partial class GroupGoToInstance
{
    /// <summary>
    /// The player's current group, by id.
    /// </summary>
    [UInt(64)] public ulong GroupId { get; set; }
}
