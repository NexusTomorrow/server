using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.PlayerPath;

[WireMessage(GameMessageOpcode.Server0671, Direction.ServerToClient)]
public sealed partial class Server0671
{
    [UInt(14)] public ushort Unknown0 { get; set; }
    public Server066EPart0 Unknown1 { get; set; }
}
