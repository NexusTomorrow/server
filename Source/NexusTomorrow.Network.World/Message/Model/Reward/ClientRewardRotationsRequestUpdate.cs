using NexusTomorrow.Game.Static.Reward;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Reward;

[WireMessage(GameMessageOpcode.Client07CC, Direction.ClientToServer)]
public sealed partial class Client07CC
{
    [UInt(32)] public uint Unknown0 { get; private set; }
}
