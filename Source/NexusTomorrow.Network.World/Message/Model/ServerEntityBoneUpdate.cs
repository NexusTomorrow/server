using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

[WireMessage(GameMessageOpcode.ServerEntityBoneUpdate, Direction.ServerToClient)]
public sealed partial class ServerEntityBoneUpdate
{
    [UInt(32)] public uint UnitId { get; set; }
    [Array(CountBits = 32), Float32] public List<float> Bones { get; set; } = [];
}
