using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Abilities;

[WireMessage(GameMessageOpcode.Server01A1, Direction.ServerToClient)]
public sealed partial class Server01A1
{
    [UInt(16)] public ushort Unknown0 { get; set; }
}
