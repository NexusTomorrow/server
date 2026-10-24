using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using StaticOption = NexusTomorrow.Game.Static.Option;

namespace NexusTomorrow.Network.World.Message.Model.Option;

/// <summary>
/// Reports the key binding set the player chose. On entering a world, the client takes the set in use from
/// <see cref="WorldEntry.PlayerState.InputKeySet"/>.
/// </summary>
[WireMessage(GameMessageOpcode.Client0571, Direction.ClientToServer)]
[Verified(Rung.Fields)]
public sealed partial class InputKeySetSelect
{
    /// <summary>
    /// The set now in use, as GameLib.SetCurrInputKeySet takes it. <see cref="StaticOption.InputKeySet.Account"/> and
    /// <see cref="StaticOption.InputKeySet.Character"/> are the sets whose bindings the server stores.
    /// </summary>
    [VerifiedMember, UInt(32)] public StaticOption.InputKeySet KeySet { get; private set; }
}
