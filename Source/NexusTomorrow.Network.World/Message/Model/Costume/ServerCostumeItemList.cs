using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Costume
{
    [Message(GameMessageOpcode.ServerCostumeItemList)]
    public class ServerCostumeItemList : IWritable
    {
        public List<uint> Item2Ids { get; set; } = [];

        public void Write(GamePacketWriter writer)
        {
            writer.Write(Item2Ids.Count);
            Item2Ids.ForEach(i => writer.Write(i));
        }
    }
}
