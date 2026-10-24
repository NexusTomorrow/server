using System.Collections.Immutable;
using NexusTomorrow.Network.World.Message.Model.Chat;

namespace NexusTomorrow.Gateway.World;

/// <summary>
/// What the server reads from a <see cref="ChatSend"/>, read to <c>Purpose</c>: the members <see cref="Members"/> names, each marked
/// verified, and nothing else. The opcode table admits the message for that reason.
/// </summary>
public static class ChatReads
{
    /// <summary>
    /// Every member the world connection reads from a <see cref="ChatSend"/>: the channel's type, the line and the client's id for it.
    /// </summary>
    public static ImmutableArray<(Type Type, string Member)> Members { get; } =
    [
        (typeof(ChatSend), nameof(ChatSend.Channel)),
        (typeof(ChatChannelKey), nameof(ChatChannelKey.Type)),
        (typeof(ChatSend), nameof(ChatSend.Text)),
        (typeof(ChatSend), nameof(ChatSend.ChatId)),
    ];
}
