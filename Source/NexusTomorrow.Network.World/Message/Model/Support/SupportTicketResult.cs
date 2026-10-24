using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Support;

/// <summary>
/// Reports whether the player's support ticket went through; the client raises AccountSupportTicketResult, which the chat log shows.
/// </summary>
[WireMessage(GameMessageOpcode.Server0972, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class SupportTicketResult
{
    /// <summary>
    /// True when the ticket was submitted, false when it failed.
    /// </summary>
    [UInt(1)] public bool Success { get; set; }
}
