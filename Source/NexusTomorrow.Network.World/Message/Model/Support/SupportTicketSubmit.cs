using System.Numerics;
using NexusTomorrow.Game.Static;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Support;

/// <summary>
/// Submits a support ticket: the SubmitSupportTicket confirm action's request, which the client sends only once both categories are
/// set.
/// </summary>
[WireMessage(GameMessageOpcode.Client06D1, Direction.ClientToServer)]
[Verified(Rung.Purpose)]
public sealed partial class SupportTicketSubmit
{
    /// <summary>
    /// The ticket's category, one of the ids PlayerTicket_GetErrorTypeList lists; the client never sends -1.
    /// </summary>
    [UInt(16)] public ushort CategoryId { get; private set; }

    /// <summary>
    /// The ticket's subcategory, one under <see cref="CategoryId"/> as PlayerTicket_GetSubtype lists them; the client never sends -1.
    /// </summary>
    [UInt(16)] public ushort SubcategoryId { get; private set; }

    /// <summary>
    /// The player unit's position.
    /// </summary>
    [Float32] public Vector3 Position { get; private set; }

    [WideString] public string Unknown3 { get; private set; } = "";
    [WideString] public string Unknown4 { get; private set; } = "";

    /// <summary>
    /// The client's language.
    /// </summary>
    [UInt(32)] public Language Language { get; private set; }
}
