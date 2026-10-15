using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

[WireMessage(GameMessageOpcode.ServerUnlockMount, Direction.ServerToClient)]
public sealed partial class ServerUnlockMount
{
    [UInt(18)] public uint Spell4Id { get; set; }
}

