using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Chat;

/// <summary>
/// Reports that a whisper could not be delivered.
/// </summary>
/// <remarks>
/// The client raises ChatTellFailed on the whisper channel, or on the account whisper channel.
/// </remarks>
[WireMessage(GameMessageOpcode.Server01D9, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class ChatWhisperFailed
{
    /// <summary>
    /// The player the whisper was for, by name.
    /// </summary>
    [WideString] public string TargetName { get; set; } = "";

    /// <summary>
    /// True for an account whisper.
    /// </summary>
    [UInt(1)] public bool IsAccountWhisper { get; set; }

    /// <summary>
    /// The line the result is for, by the slot the client filed it under when it sent it; the client frees the slot.
    /// </summary>
    [UInt(16)] public ushort ChatId { get; set; }
}
