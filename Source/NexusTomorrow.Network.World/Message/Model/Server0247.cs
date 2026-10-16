using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Combat;

namespace NexusTomorrow.Network.World.Message.Model;

[WireMessage(GameMessageOpcode.Server0247, Direction.ServerToClient)]
public partial class Server0247
{
    [UInt(6)] public byte Unknown0 { get; set; }
    [Union(nameof(Unknown0))] public IServer0247Union0 Unknown1 { get; set; } = new Server0247Union0Arm00();
}
