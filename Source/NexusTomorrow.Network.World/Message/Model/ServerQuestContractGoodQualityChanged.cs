using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

[WireMessage(GameMessageOpcode.ServerQuestContractGoodQualityChanged, Direction.ServerToClient)]
public sealed partial class ServerQuestContractGoodQualityChanged
{
    [FixedArray(3), UInt(32)] public uint[] QuestIds { get; set; } = new uint[3];
}
