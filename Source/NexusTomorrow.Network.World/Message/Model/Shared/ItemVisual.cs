using NexusTomorrow.Game.Static.Entity;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Shared
{
    public class ItemVisual : IWritable
    {
        public ItemSlot Slot { get; set; }
        public ushort DisplayId { get; set; }
        public ushort ColourSetId { get; set; }
        public uint DyeData { get; set; }

        public void Write(GamePacketWriter writer)
        {
            writer.Write(Slot, 7);
            writer.Write(DisplayId, 15);
            writer.Write(ColourSetId, 14);
            writer.Write(DyeData);
        }
    }
}
