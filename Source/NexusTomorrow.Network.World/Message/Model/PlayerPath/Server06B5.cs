using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.PlayerPath;

[WireMessage(GameMessageOpcode.Server06B5, Direction.ServerToClient)]
public sealed partial class Server06B5
{
    [UInt(14)] public ushort Unknown0 { get; set; }
    [Array(CountBits = 16)] public List<Server06B5Part0> Unknown1 { get; set; } = [];
}
