using NexusTomorrow.Network.Message;
using NexusTomorrow.Game.Static.Crafting;
using System.Numerics;

namespace NexusTomorrow.Network.World.Message.Model.Crafting
{
    [Message(GameMessageOpcode.ServerSchematicAddLearned)]
    public class ServerSchematicAddLearned : IWritable
    {
        public TradeskillType TradeskillId { get; set; }
        public uint TradeskillSchematic2Id { get; set; }
        public Vector2 DiscoveryCoordinates { get; set; } // Coordinates on the discovery panel

        public void Write(GamePacketWriter writer)
        {
            writer.Write(TradeskillId, 32u);
            writer.Write(TradeskillSchematic2Id);
            writer.Write(DiscoveryCoordinates.X);
            writer.Write(DiscoveryCoordinates.Y);
        }
    }
}
