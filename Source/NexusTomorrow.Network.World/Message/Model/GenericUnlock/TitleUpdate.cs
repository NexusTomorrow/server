using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.GenericUnlock;

/// <summary>
/// Adds or removes a character title.
/// </summary>
/// <remarks>
/// The client looks up the title record; when not removing it is added with its lifetime, otherwise dropped from the list.
/// </remarks>
[WireMessage(GameMessageOpcode.Server018A, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class TitleUpdate
{
    /// <summary>
    /// The title, by its id in the CharacterTitle table.
    /// </summary>
    [VerifiedMember, UInt(14)] public ushort TitleId { get; set; }

    /// <summary>
    /// When set, the title text is looked up with the prefix variant.
    /// </summary>
    [VerifiedMember, UInt(1)] public bool IsPrefix { get; set; }

    /// <summary>
    /// When clear, the title is added; when set, it is removed.
    /// </summary>
    [VerifiedMember, UInt(1)] public bool Remove { get; set; }
}