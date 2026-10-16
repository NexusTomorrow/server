using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Support;

[WireMessage(GameMessageOpcode.Client0830, Direction.ClientToServer)]
public sealed partial class Client0830
{
    [UInt(16)] public ushort Unknown0 { get; private set; }
    [UInt(32)] public uint Unknown1 { get; private set; }
    [UInt(32)] public uint Unknown2 { get; private set; }
    [WideString] public string Unknown3 { get; private set; } = "";
}
