using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Chat;

/// <summary>
/// Whispers a chat line to a player or, by the account whisper channel, to an account friend.
/// </summary>
/// <remarks>
/// The server answers a failed whisper with <see cref="ChatWhisperFailed"/>, which returns the name and the bit.
/// </remarks>
[WireMessage(GameMessageOpcode.Client01D4, Direction.ClientToServer)]
[Verified(Rung.Purpose)]
public sealed partial class ChatWhisper
{
    /// <summary>
    /// The player the whisper is for, by name.
    /// </summary>
    [WideString] public string TargetName { get; private set; } = "";

    /// <summary>
    /// The realm the player typed after the target's name: the client splits it off the name at an at sign, a hyphen or a
    /// full stop.
    /// </summary>
    [VerifiedMember, WideString] public string TargetRealm { get; private set; } = "";

    /// <summary>
    /// The line's text.
    /// </summary>
    [WideString] public string Text { get; private set; } = "";

    /// <summary>
    /// The links in the text.
    /// </summary>
    [Array(CountBits = 5)] public List<ChatLink> Links { get; } = [];

    /// <summary>
    /// The line, by the slot the client files it under until the server answers.
    /// </summary>
    [UInt(16)] public ushort ChatId { get; set; }

    /// <summary>
    /// True for an account whisper.
    /// </summary>
    [UInt(1)] public bool IsAccountWhisper { get; set; }
}
