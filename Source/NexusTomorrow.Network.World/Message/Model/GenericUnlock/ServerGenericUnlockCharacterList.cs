using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.GenericUnlock;

// Sets bCharacterUnlocked state on all GenericUnlocks on the client.
// Seems that ServerGenericUnlocksCharacterRefreshed was the preferred method
// as that clears the bCharacterUnlocked state on everything first
[WireMessage(GameMessageOpcode.ServerGenericUnlockCharacterList, Direction.ServerToClient)]
public sealed partial class ServerGenericUnlockCharacterList
{
    [Array(CountBits = 32), UInt(32)] public List<uint> GenericUnlockEntryIds { get; set; } = [];
}
