using NexusTomorrow.Game.Static.Quest;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

[WireMessage(GameMessageOpcode.ServerQuestStateChange, Direction.ServerToClient)]
public sealed partial class ServerQuestStateChange
{
    [UInt(15)] public ushort QuestId { get; set; }
    [UInt(32)] public QuestState QuestState { get; set; }
    [UInt(32)] public uint QuestObjectiveId { get; set; }
}
