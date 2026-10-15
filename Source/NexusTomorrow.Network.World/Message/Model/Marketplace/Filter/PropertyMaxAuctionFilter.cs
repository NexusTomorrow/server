using NexusTomorrow.Game.Static.Entity;

namespace NexusTomorrow.Network.World.Message.Model.Marketplace.Filter
{
    public class PropertyMaxAuctionFilter : IAuctionFilter
    {
        public Property UnitProperty2Id { get; private set; }
        public float Maximum { get; private set; }

        public void Read(GamePacketReader reader)
        {
            UnitProperty2Id = reader.ReadEnum<Property>(8u);
            Maximum = reader.ReadSingle();
        }
    }
}
