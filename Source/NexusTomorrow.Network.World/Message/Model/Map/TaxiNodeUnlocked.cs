using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Map;

[WireMessage(GameMessageOpcode.Server0186, Direction.ServerToClient)]
public sealed partial class TaxiNodeUnlocked
{
    [UInt(14)] public ushort Unknown0 { get; set; }
}
