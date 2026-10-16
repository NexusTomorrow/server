using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Pregame;

[WireMessage(GameMessageOpcode.Client0352, Direction.ClientToServer)]
public sealed partial class Client0352
{
    [UInt(64)] public ulong Unknown0 { get; private set; }
}
