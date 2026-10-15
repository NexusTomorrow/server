using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.GenericUnlock;

// Clears the bCharacterUnlocked state on all GenericUnlocks on the client then
// uses the provided array to set the state again. Seems to be used preferentially
// over ServerGenericUnlockCharacterList (0x0983).
[Message(GameMessageOpcode.ServerGenericUnlockCharacterRefresh)]
[WireMessage(GameMessageOpcode.ServerGenericUnlockCharacterRefresh, Direction.ServerToClient)]
public sealed partial class ServerGenericUnlockCharacterRefresh : IWritable
{
    [Array(CountBits = 32), UInt(32)] public List<uint> GenericUnlockEntryIds { get; set; } = [];

    public void Write(GamePacketWriter writer)
    {
        writer.Write(GenericUnlockEntryIds.Count);
        GenericUnlockEntryIds.ForEach(id => writer.Write(id));
    }
}
