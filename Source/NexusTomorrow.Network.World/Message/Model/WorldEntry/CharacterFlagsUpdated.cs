using NexusTomorrow.Game.Static.Entity;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.WorldEntry;

/// <summary>
/// The character's settings for the friendship block, ignoring duel requests and the guild holomark's display, as the server keeps
/// them. The client keeps the mask and raises CharacterFlagsUpdated for its add-ons. Its setters for these settings test the mask before
/// they ask the server for a change.
/// </summary>
[WireMessage(GameMessageOpcode.Server00FE, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class CharacterFlagsUpdated
{
    /// <summary>
    /// The settings, a <see cref="CharacterFlag"/> mask. The client reads no bit that <see cref="CharacterFlag"/> does not name.
    /// </summary>
    [VerifiedMember, UInt(32)] public CharacterFlag Flags { get; set; }
}
