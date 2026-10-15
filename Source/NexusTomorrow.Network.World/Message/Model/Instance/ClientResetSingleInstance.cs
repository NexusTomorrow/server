using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Instance
{
    [Message(GameMessageOpcode.ClientResetSingleInstance)]
    public class ClientResetSingleInstance : IReadable
    {
        public uint InstancePortalUnitId { get; private set; }

        public void Read(GamePacketReader reader)
        {
            InstancePortalUnitId = reader.ReadUInt();
        }
    }
}
