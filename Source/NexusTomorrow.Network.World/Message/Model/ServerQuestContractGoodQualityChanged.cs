using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

[Message(GameMessageOpcode.ServerQuestContractGoodQualityChanged)]
[WireMessage(GameMessageOpcode.ServerQuestContractGoodQualityChanged, Direction.ServerToClient)]
public sealed partial class ServerQuestContractGoodQualityChanged : IWritable
{
    [FixedArray(3), UInt(32)] public uint[] QuestIds { get; set; } = new uint[3];

    public void Write(GamePacketWriter writer)
    {
        foreach (uint id in QuestIds)
        {
            writer.Write(id);
        }
    }
}
