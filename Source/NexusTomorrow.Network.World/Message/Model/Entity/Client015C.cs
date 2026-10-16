using NexusTomorrow.Game.Static.Entity.Movement;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Entity;

[WireMessage(GameMessageOpcode.Client015C, NexusTomorrow.Network.Codec.Direction.ClientToServer)]
public sealed partial class Client015C
{
    [UInt(3)] public byte Unknown0 { get; private set; }
}
