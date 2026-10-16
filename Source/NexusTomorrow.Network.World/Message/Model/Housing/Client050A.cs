using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Housing;

[WireMessage(GameMessageOpcode.Client050A, Direction.ClientToServer)]
public sealed partial class Client050A
{
    public Client00B5Part0 Unknown0 { get; } = new();
    [UInt(32)] public uint Unknown1 { get; private set; }
    [UInt(32)] public uint Unknown2 { get; private set; }
    [UInt(32)] public uint Unknown3 { get; private set; }
    [UInt(32)] public uint Unknown4 { get; private set; }
    [UInt(32)] public uint Unknown5 { get; private set; }
    [UInt(32)] public uint Unknown6 { get; private set; }
    [UInt(32)] public uint Unknown7 { get; private set; }
    [UInt(3)] public byte Unknown8 { get; private set; }
}
