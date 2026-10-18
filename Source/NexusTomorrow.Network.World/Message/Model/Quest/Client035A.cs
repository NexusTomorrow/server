using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Quest;

[WireMessage(GameMessageOpcode.Client035A, Direction.ClientToServer)]
public sealed partial class Client035A
{
    [UInt(15)] public ushort Unknown0 { get; private set; }
}
