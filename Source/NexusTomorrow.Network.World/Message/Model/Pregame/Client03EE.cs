using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Pregame;

[WireMessage(GameMessageOpcode.Client03EE, Direction.ClientToServer)]
public sealed partial class Client03EE
{
    [UInt(1)] public bool Unknown0 { get; private set; }
}
