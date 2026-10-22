using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Guild;

/// <summary>
/// Subscribes to guild recruitment, or unsubscribes: the request GuildLib.RecruitmentSubscribe and
/// GuildLib.RecruitmentUnsubscribe send.
/// </summary>
/// <remarks>
/// Subscribing also starts a 120000 ms timer.
/// </remarks>
[WireMessage(GameMessageOpcode.Client076F, Direction.ClientToServer)]
[Verified(Rung.Fields)]
public sealed partial class GuildRecruitmentSubscribe
{
    /// <summary>
    /// True from GuildLib.RecruitmentSubscribe, false from GuildLib.RecruitmentUnsubscribe.
    /// </summary>
    [UInt(1)] public bool Subscribed { get; private set; }
}
