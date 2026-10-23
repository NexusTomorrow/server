using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Entity;

[WireMessage(GameMessageOpcode.Client00C3, Direction.ClientToServer)]
public sealed partial class StunBreakoutInput
{
    [UInt(8)] public byte Unknown0 { get; private set; }
    [UInt(8)] public byte Unknown1 { get; private set; }
}
