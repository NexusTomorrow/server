using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Reward;

[WireMessage(GameMessageOpcode.Client0865, Direction.ClientToServer)]
public sealed partial class Client0865
{
    [UInt(64)] public ulong Unknown0 { get; set; }
}
