using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Guild;

/// <summary>
/// Answers a guild invitation: the request GuildLib.Accept and GuildLib.Decline send.
/// </summary>
[WireMessage(GameMessageOpcode.Client0490, Direction.ClientToServer)]
[Verified(Rung.Fields)]
public sealed partial class GuildInviteAnswer
{
    /// <summary>
    /// True from GuildLib.Accept, false from GuildLib.Decline.
    /// </summary>
    [UInt(1)] public bool Accepted { get; private set; }
}
