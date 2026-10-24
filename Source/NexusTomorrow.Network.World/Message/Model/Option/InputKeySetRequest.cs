using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Option;

/// <summary>
/// Asks for the stored bindings of one key binding set. The client sends it when it loads the account's or the character's set and holds
/// no list for that owner. The server answers with <see cref="InputKeySet"/> for the owner, an empty list included: the key binding window
/// waits for that answer.
/// </summary>
[WireMessage(GameMessageOpcode.Client0570, Direction.ClientToServer)]
[Verified(Rung.Fields)]
public sealed partial class InputKeySetRequest
{
    /// <summary>
    /// The set's owner, as <see cref="InputKeySet.CharacterId"/> names one: 0 for the account's set, or the character's id.
    /// </summary>
    [VerifiedMember, UInt(64)] public ulong CharacterId { get; private set; }
}
