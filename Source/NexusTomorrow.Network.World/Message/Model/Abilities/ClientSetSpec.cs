using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Abilities;

[WireMessage(GameMessageOpcode.Client0174, Direction.ClientToServer)]
public sealed partial class Client0174
{
    [UInt(3)] public byte Unknown0 { get; private set; }
}
