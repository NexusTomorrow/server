using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Unsettled;

[WireMessage(GameMessageOpcode.Client04ED, Direction.ClientToServer)]
public sealed partial class Client04ED
{
    [UInt(2)] public byte Unknown0 { get; set; }
    [WideString] public string Unknown1 { get; set; } = "";
}
