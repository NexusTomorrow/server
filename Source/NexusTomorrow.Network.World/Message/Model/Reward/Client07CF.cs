using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Reward;

[WireMessage(GameMessageOpcode.Client07CF, Direction.ClientToServer)]
public sealed partial class Client07CF
{
    [UInt(14)] public ushort Unknown0 { get; private set; }
}
