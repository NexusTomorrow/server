using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Housing
{
    [Message(GameMessageOpcode.ServerHousingAttachUnitToDecorItem)]
    public class ServerHousingAttachUnitToDecorItem : IWritable
    {
        public Identity ResidenceIdentity { get; set; }
        public ulong DecorId { get; set; }
        public ulong AttachedUnitId { get; set; }

        public void Write(GamePacketWriter writer)
        {
            ResidenceIdentity.Write(writer);
            writer.Write(DecorId);
            writer.Write(AttachedUnitId);
        }
    }
}
