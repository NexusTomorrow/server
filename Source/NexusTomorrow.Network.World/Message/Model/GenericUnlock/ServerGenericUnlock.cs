using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.GenericUnlock
{
    [Message(GameMessageOpcode.ServerGenericUnlockAccount)]
    public class ServerGenericUnlock : IWritable
    {
        public ushort GenericUnlockEntryId { get; set; }

        public void Write(GamePacketWriter writer)
        {
            writer.Write(GenericUnlockEntryId, 14u);
        }
    }
}
