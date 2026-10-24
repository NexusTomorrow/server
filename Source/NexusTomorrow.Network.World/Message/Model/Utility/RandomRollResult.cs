using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Utility;

/// <summary>
/// A random roll's result, which the client writes as a line of the chat log. The client shows a roll only by a roller it can name: the
/// player, or a member of the player's group.
/// </summary>
[WireMessage(GameMessageOpcode.Message071C, Direction.ServerToClient)]
[Verified(Rung.Purpose)]
public sealed partial class RandomRollResult
{
    /// <summary>
    /// The roller, by realm and character id. The client takes the player's name when it is the player's own id, and otherwise the name
    /// of the group member it names; with neither, it shows nothing.
    /// </summary>
    [VerifiedMember] public RealmScopedId CharacterId { get; set; } = new();

    [UInt(32)] public uint Unknown1 { get; set; }
    [UInt(32)] public uint Unknown2 { get; set; }
    [UInt(32)] public uint Unknown3 { get; set; }
}
