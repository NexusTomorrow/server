using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Mail;

[WireMessage(GameMessageOpcode.Client0168, Direction.ClientToServer)]
public sealed partial class Client0168
{
    [WideString] public string Unknown0 { get; private set; } = "";
    [WideString] public string Unknown1 { get; private set; } = "";
    [WideString] public string Unknown2 { get; private set; } = "";
    [WideString] public string Unknown3 { get; private set; } = "";
    [UInt(64)] public ulong Unknown4 { get; private set; }
    [UInt(64)] public ulong Unknown5 { get; private set; }
    [UInt(2)] public byte Unknown6 { get; private set; }
    [UInt(32)] public uint Unknown7 { get; private set; }
    [FixedArray(10), UInt(64)] public ulong[] Unknown8 { get; private set; } = new ulong[10];
}
