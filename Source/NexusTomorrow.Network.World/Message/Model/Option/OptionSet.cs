using NexusTomorrow.Game.Static.Option;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Option;

/// <summary>
/// Reports one of the player's options: the casting options each time one of their console variables changes, or the share preference
/// for challenges each time the interface sets it.
/// </summary>
[WireMessage(GameMessageOpcode.Client012B, Direction.ClientToServer)]
[Verified(Rung.Fields)]
public sealed partial class OptionSet
{
    /// <summary>
    /// Which option <see cref="Value"/> carries.
    /// </summary>
    [VerifiedMember, UInt(32)] public OptionSetType Type { get; private set; }

    /// <summary>
    /// The option. With <see cref="OptionSetType.InputFlags"/>, the client's whole <see cref="InputFlags"/> mask after the change. With
    /// <see cref="OptionSetType.SharedChallengePreference"/>, the <see cref="SharedChallengePreference"/> the interface passes to
    /// GameLib.SetSharedChallengePreference.
    /// </summary>
    [VerifiedMember, UInt(32)] public uint Value { get; private set; }
}
