using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Chat;

/// <summary>
/// Delivers a chat line on a channel.
/// </summary>
/// <remarks>
/// The client drops a line from a sender it ignores, and an account whisper from someone not on its account friends list; it
/// files the rest for reporting and raises ChatMessage.
/// </remarks>
[WireMessage(GameMessageOpcode.Server01C8, Direction.ServerToClient)]
[Verified(Rung.Purpose)]
public sealed partial class ChatMessageReceived
{
    /// <summary>
    /// The channel.
    /// </summary>
    [VerifiedMember] public ChatChannelKey Channel { get; set; } = new();

    /// <summary>
    /// True when the sender is a game master.
    /// </summary>
    [UInt(1)] public bool IsGm { get; set; }

    /// <summary>
    /// True for the player's own line.
    /// </summary>
    [UInt(1)] public bool IsSelf { get; set; }

    /// <summary>
    /// True for an automatic response.
    /// </summary>
    [UInt(1)] public bool IsAutoResponse { get; set; }

    /// <summary>
    /// The sender, by character id: the client drops a line whose sender its ignore list holds.
    /// </summary>
    [VerifiedMember] public RealmScopedId SenderId { get; set; } = new();

    /// <summary>
    /// The sender, by name.
    /// </summary>
    [VerifiedMember, WideString] public string SenderName { get; set; } = "";

    /// <summary>
    /// The sender's realm, by name.
    /// </summary>
    [WideString] public string RealmName { get; set; } = "";

    /// <summary>
    /// The sender's presence state.
    /// </summary>
    [UInt(3)] public byte PresenceState { get; set; }

    /// <summary>
    /// The line's text.
    /// </summary>
    [VerifiedMember, WideString] public string Text { get; set; } = "";

    /// <summary>
    /// The links in the text.
    /// </summary>
    [Array(CountBits = 5)] public List<ChatLink> Links { get; set; } = [];

    /// <summary>
    /// True when the line comes from the other faction.
    /// </summary>
    [UInt(1)] public bool CrossFaction { get; set; }

    /// <summary>
    /// The client gives this value no meaning.
    /// </summary>
    [UInt(16)] public ushort Unknown11 { get; set; }

    /// <summary>
    /// The unit the line comes from, by unit id, which its speech bubble rises over on channels that show one; 0 names no unit.
    /// </summary>
    [VerifiedMember, UInt(32)] public ulong UnitId { get; set; }

    [UInt(8)] public byte Unknown13 { get; set; }
}
