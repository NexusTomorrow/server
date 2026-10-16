using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Friendship;

[WireMessage(GameMessageOpcode.Client03B4, Direction.ClientToServer)]
public sealed partial class Client03B4
{
    [WideString] public string Unknown0 { get; private set; } = "";
    [WideString] public string Unknown1 { get; private set; } = "";
    [UInt(4)] public byte Unknown2 { get; private set; }
    [WideString] public string Unknown3 { get; private set; } = "";
}
