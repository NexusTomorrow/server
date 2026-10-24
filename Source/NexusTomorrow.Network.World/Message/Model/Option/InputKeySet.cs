using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using StaticOption = NexusTomorrow.Game.Static.Option;

namespace NexusTomorrow.Network.World.Message.Model.Option;

/// <summary>
/// The key bindings of one key binding set that differ from the client's base bindings. The client sends it when the player saves the
/// set in use, and takes it from the server as that owner's stored bindings.
/// </summary>
/// <remarks>
/// A save lists every binding that differs from the base, then the base binding of each action the owner's last list held and the new
/// list lacks, which resets it. The client keeps the last list for each owner. On receipt it replaces that owner's list, and only when
/// the list is the set in use's (<see cref="StaticOption.InputKeySet.Account"/> with owner 0, or
/// <see cref="StaticOption.InputKeySet.Character"/> with the player's character id) does it apply the list over the base bindings and
/// raise KeyBindingUpdated for its add-ons.
/// </remarks>
[WireMessage(GameMessageOpcode.Message056F, Direction.Both)]
[Verified(Rung.Fields)]
public sealed partial class InputKeySet
{
    /// <summary>
    /// The bindings, each for one input action.
    /// </summary>
    [VerifiedMember, Array(CountBits = 32)] public List<InputKeyBinding> Bindings { get; set; } = [];

    /// <summary>
    /// The set's owner: 0 for the account's set, or the character's id for that character's set. The client keys its stored lists by it.
    /// </summary>
    [VerifiedMember, UInt(64)] public ulong CharacterId { get; set; }
}
