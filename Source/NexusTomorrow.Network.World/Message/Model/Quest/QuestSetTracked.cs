using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Quest;

[WireMessage(GameMessageOpcode.Client0364, Direction.ClientToServer)]
public sealed partial class QuestSetTracked
{
    [UInt(15)] public ushort Unknown0 { get; private set; }
    [UInt(1)] public bool Unknown1 { get; private set; }
}
