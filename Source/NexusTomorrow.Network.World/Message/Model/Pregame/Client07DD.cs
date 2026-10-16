using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Pregame;

[WireMessage(GameMessageOpcode.Client07DD, Direction.ClientToServer)]
public sealed partial class Client07DD
{
    [UInt(64)] public ulong Unknown0 { get; private set; }
}
