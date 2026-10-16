using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Abilities;

[WireMessage(GameMessageOpcode.Server019F, Direction.ServerToClient)]
public sealed partial class Server019F
{
    [UInt(2)] public byte Unknown0 { get; set; }
}
