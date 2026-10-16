using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

[WireMessage(GameMessageOpcode.Client023D, Direction.ClientToServer)]
public sealed partial class Client023D
{
    [UInt(6)] public uint Unknown0 { get; private set; }
    [UInt(10)] public uint Unknown1 { get; private set; }
    [UInt(64)] public ulong Unknown2 { get; private set; }
    [UInt(64)] public ulong Unknown3 { get; private set; }
    [UInt(64)] public ulong Unknown4 { get; private set; }
    [UInt(64)] public ulong Unknown5 { get; private set; }
    [UInt(64)] public ulong Unknown6 { get; private set; }
    [UInt(64)] public ulong Unknown7 { get; private set; }
    [WideString] public string Unknown8 { get; private set; }
}
