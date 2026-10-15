using NexusTomorrow.Game.Static.Group;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model
{
    [Message(GameMessageOpcode.ClientGroupMarkUnit)]
    public class ClientGroupMark : IReadable
    {
        public GroupMarker Marker { get; set; }

        public uint UnitId { get; set; }

        public void Read(GamePacketReader reader)
        {
            Marker = reader.ReadEnum<GroupMarker>(32u);
            UnitId = reader.ReadUInt();
        }
    }
}
