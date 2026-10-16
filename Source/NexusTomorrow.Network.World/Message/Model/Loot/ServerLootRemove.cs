using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Loot;

[WireMessage(GameMessageOpcode.Server08A1, Direction.ServerToClient)]
public sealed partial class Server08A1
{
    [UInt(32)] public uint Unknown0 { get; set; }
}
