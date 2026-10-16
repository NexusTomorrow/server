using NexusTomorrow.Game.Static.Housing;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Housing;

[WireMessage(GameMessageOpcode.Client00C9, Direction.ClientToServer)]
public sealed partial class Client00C9
{
    [UInt(32)] public uint Unknown0 { get; private set; }
}
