using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Marketplace;

[WireMessage(GameMessageOpcode.Server097B, Direction.ServerToClient)]
public sealed partial class CreddRedeemResult
{
    [UInt(6)] public byte Unknown0 { get; set; }
}
