using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.PublicEvent;

[WireMessage(GameMessageOpcode.Client06EE, Direction.ClientToServer)]
public sealed partial class Client06EE
{
    [UInt(14)] public uint Unknown0 { get; private set; }
    [UInt(14)] public uint Unknown1 { get; private set; }
    [UInt(14)] public uint Unknown2 { get; private set; }
    [UInt(32)] public uint Unknown3 { get; private set; }
}
