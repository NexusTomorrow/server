using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

[WireMessage(GameMessageOpcode.ServerQuestShared, Direction.ServerToClient)]
public sealed partial class ServerQuestShared
{
    [UInt(32)] public uint SharerUnitId { get; set; }
    [UInt(15)] public uint QuestId { get; set; }
}
