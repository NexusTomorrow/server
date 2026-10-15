using NexusTomorrow.Game.Static.Option;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Option;

// When Type is Casting, NewValue is a bitmask of CastingOptionFlags.
// When Type is SharedChallenge, NewValue only indicates whether the player wants to allow shared challenges.
[WireMessage(GameMessageOpcode.ClientOptions, Direction.ClientToServer)]
public sealed partial class ClientOptions
{
    [UInt(32)] public OptionType Type { get; private set; }
    [UInt(32)] public uint NewValue { get; private set; }
}
