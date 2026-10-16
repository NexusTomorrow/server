using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.PlayerPath;

[WireMessage(GameMessageOpcode.Server01AA, Direction.ServerToClient)]
public sealed partial class Server01AA
{
    [UInt(32)] public uint Unknown0 { get; set; }
}
