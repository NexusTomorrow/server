using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Housing;

[WireMessage(GameMessageOpcode.Client0510, Direction.ClientToServer)]
public sealed partial class Client0510
{
    [WireStruct]
    public sealed partial class Client0510Part0
    {
        [UInt(32)] public uint Unknown0 { get; private set; }
        [UInt(32)] public uint Unknown1 { get; private set; }
        [UInt(32)] public uint Unknown2 { get; private set; }
        [UInt(32)] public uint Unknown3 { get; private set; }
        [UInt(32)] public uint Unknown4 { get; private set; }
    }

    public RealmScopedId Unknown0 { get; private set; } = new();
    [UInt(32)] public uint Unknown1 { get; private set; }
    [UInt(32)] public uint Unknown2 { get; private set; }
    [UInt(32)] public uint Unknown3 { get; private set; }
    [UInt(32)] public uint Unknown4 { get; private set; }
    [UInt(3)] public byte Unknown5 { get; private set; }
    [FixedArray(5)] public Client0510Part0[] Unknown6 { get; private set; } = [.. Enumerable.Range(0, 5).Select(_ => new Client0510Part0())];
}
