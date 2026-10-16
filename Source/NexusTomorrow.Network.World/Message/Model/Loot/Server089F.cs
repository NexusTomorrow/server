using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Loot;

[WireMessage(GameMessageOpcode.Server089F, Direction.ServerToClient)]
public sealed partial class Server089F
{
    [UInt(32)] public uint Unknown0 { get; set; }
}
