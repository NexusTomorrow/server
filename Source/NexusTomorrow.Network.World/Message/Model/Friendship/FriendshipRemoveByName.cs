using NexusTomorrow.Game.Static.Friendship;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Friendship;

/// <summary>
/// Removes a friendship with a character, by the character's name.
/// </summary>
[WireMessage(GameMessageOpcode.Client03C2, Direction.ClientToServer)]
[Verified(Rung.Purpose)]
public sealed partial class FriendshipRemoveByName
{
    [WideString] public string Unknown0 { get; private set; } = "";
    [WideString] public string Unknown1 { get; private set; } = "";

    /// <summary>
    /// The kind of friendship to remove.
    /// </summary>
    [UInt(4)] public FriendshipType Type { get; private set; }
}
