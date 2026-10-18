using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Unsettled;

[WireMessage(GameMessageOpcode.Client0825, Direction.ClientToServer)]
public sealed partial class Client0825
{
    [UInt(32)] public uint Unknown0 { get; set; }
    [UInt(32)] public uint Unknown1 { get; set; }
    [UInt(32)] public uint Unknown2 { get; set; }
    [WideString] public string Unknown3 { get; set; } = "";
    [WideString] public string Unknown4 { get; set; } = "";
}
