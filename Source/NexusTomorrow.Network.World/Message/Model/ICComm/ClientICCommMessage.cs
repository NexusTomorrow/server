using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.ICComm;

[WireMessage(GameMessageOpcode.Client054B, Direction.ClientToServer)]
public sealed partial class Client054B
{
    [UInt(64)] public ulong Unknown0 { get; private set; }
    [UInt(32)] public uint Unknown1 { get; private set; }
    [NarrowString] public string Unknown2 { get; private set; }
    [WideString] public string Unknown3 { get; private set; }
}
