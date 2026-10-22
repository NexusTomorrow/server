using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Guild;

[WireMessage(GameMessageOpcode.Client0473, Direction.ClientToServer)]
public sealed partial class Client0473
{
    public RealmScopedId Unknown0 { get; private set; } = new RealmScopedId();
    [UInt(64)] public ulong Unknown1 { get; private set; }
    [UInt(64)] public ulong Unknown2 { get; private set; }
    [UInt(18)] public uint Unknown3 { get; private set; }
    public ItemLocation Unknown4 { get; private set; } = new ItemLocation();
    public ItemLocation Unknown5 { get; private set; } = new ItemLocation();
    [UInt(1)] public bool Unknown6 { get; private set; }
    [UInt(32)] public uint Unknown7 { get; private set; }
}
