using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Chat;

/// <summary>
/// Accepts a chat line the player sent: the client posts its own copy, as <see cref="ChatMessageReceived"/> would.
/// </summary>
/// <remarks>
/// The client swaps in each item record for the inventory item link with the same id, and frees the line's slot.
/// </remarks>
[WireMessage(GameMessageOpcode.Server01C2, Direction.ServerToClient)]
[Verified(Rung.Purpose)]
public sealed partial class ChatSendAccepted
{
    /// <summary>
    /// The line, by the slot the client files it under until the server answers.
    /// </summary>
    [VerifiedMember, UInt(16)] public ushort ChatId { get; set; }

    /// <summary>
    /// True when the player is a game master.
    /// </summary>
    [UInt(1)] public bool IsGm { get; set; }

    /// <summary>
    /// The line's inventory items, described in full.
    /// </summary>
    [Array(CountBits = 5)] public List<ChatItemRecord> Items { get; } = [];

    /// <summary>
    /// The speaker's name: the client shows the player's own accepted line under this name, never the player's own.
    /// </summary>
    [VerifiedMember, WideString] public string SenderName { get; set; } = "";

    /// <summary>
    /// The speaker's realm, by name, which the client shows with the line.
    /// </summary>
    [VerifiedMember, WideString] public string RealmName { get; set; } = "";

    /// <summary>
    /// The speaker's unit, by unit id: the client ties the line to the player's unit when this is the player's id, and to no unit for
    /// 0.
    /// </summary>
    [VerifiedMember, UInt(32)] public uint UnitId { get; set; }

    [UInt(8)] public byte Unknown6 { get; set; }
}
