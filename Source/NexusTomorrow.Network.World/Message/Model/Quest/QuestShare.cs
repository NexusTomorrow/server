using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Quest;

[WireMessage(GameMessageOpcode.Client045F, Direction.ClientToServer)]
public sealed partial class QuestShare
{
    [UInt(15)] public ushort Unknown0 { get; private set; }
}
