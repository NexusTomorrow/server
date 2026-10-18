using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Abilities;

[WireMessage(GameMessageOpcode.Server0567, Direction.ServerToClient)]
public sealed partial class Server0567
{
    [UInt(64)] public ulong Unknown0 { get; set; }
    [UInt(8)] public byte Unknown1 { get; set; }
}
