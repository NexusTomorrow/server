using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Instance;

[WireMessage(GameMessageOpcode.Server0155, Direction.ServerToClient)]
public sealed partial class InstanceResetResult
{
    [UInt(15)] public uint Unknown0 { get; set; }
    [UInt(1)] public bool Unknown1 { get; set; }
}
