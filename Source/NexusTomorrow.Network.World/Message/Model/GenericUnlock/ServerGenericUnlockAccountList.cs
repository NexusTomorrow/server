using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.GenericUnlock;

[Message(GameMessageOpcode.ServerGenericUnlockAccountList)]
[WireMessage(GameMessageOpcode.ServerGenericUnlockAccountList, Direction.ServerToClient)]
public sealed partial class ServerGenericUnlockAccountList : IWritable
{
    [Array(CountBits = 32), UInt(32)] public List<uint> GenericUnlockEntryIds { get; set; } = [];

    public void Write(GamePacketWriter writer)
    {
        writer.Write(GenericUnlockEntryIds.Count);
        GenericUnlockEntryIds.ForEach(id => writer.Write(id));
    }
}
