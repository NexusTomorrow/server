using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.GenericUnlock;

[Message(GameMessageOpcode.ServerGenericUnlockAccount)]
[WireMessage(GameMessageOpcode.ServerGenericUnlockAccount, Direction.ServerToClient)]
public sealed partial class ServerGenericUnlock : IWritable
{
    [UInt(14)] public ushort GenericUnlockEntryId { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(GenericUnlockEntryId, 14u);
    }
}
