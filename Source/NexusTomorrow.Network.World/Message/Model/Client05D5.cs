using NexusTomorrow.Game.Static.Matching;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

[WireMessage(GameMessageOpcode.Client05D5, Direction.ClientToServer)]
public sealed partial class Client05D5
{
    [UInt(32)] public uint Unknown0 { get; private set; }
}
