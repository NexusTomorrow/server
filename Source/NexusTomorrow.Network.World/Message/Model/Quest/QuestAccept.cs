using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Quest;

[WireMessage(GameMessageOpcode.Client035B, Direction.ClientToServer)]
public sealed partial class QuestAccept
{
    public Server0111Part2 Unknown0 { get; } = new();
    [UInt(15)] public ushort Unknown1 { get; private set; }
}
