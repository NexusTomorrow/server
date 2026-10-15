using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

// Sets the WorldLocation2Id for a specific quest objective
[WireMessage(GameMessageOpcode.ServerQuestObjectiveWorldLocation, Direction.ServerToClient)]
public sealed partial class ServerQuestObjectiveWorldLocation
{
    [UInt(15)] public uint QuestId { get; set; }
    [UInt(32)] public uint QuestObjectiveIndex { get; set; }
    [UInt(17)] public uint WorldLocation2Id { get; set; }
}
