using NexusTomorrow.Game.Static.Option;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Option;

[WireMessage(GameMessageOpcode.Client0571, Direction.ClientToServer)]
public sealed partial class Client0571
{
    [UInt(32)] public uint Unknown0 { get; private set; }
}
