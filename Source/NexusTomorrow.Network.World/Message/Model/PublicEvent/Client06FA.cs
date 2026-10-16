using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.PublicEvent;

[WireMessage(GameMessageOpcode.Client06FA, Direction.ClientToServer)]
public sealed partial class Client06FA
{
    [UInt(14)] public uint Unknown0 { get; private set; }
    [UInt(1)] public bool Unknown1 { get; private set; }
}
