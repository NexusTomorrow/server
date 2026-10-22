using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Guild;

[WireMessage(GameMessageOpcode.Client0471, Direction.ClientToServer)]
public sealed partial class Client0471
{
    public RealmScopedId Unknown0 { get; private set; } = new RealmScopedId();
    [UInt(64)] public ulong Unknown1 { get; private set; }
    [UInt(64)] public ulong Unknown2 { get; private set; }
    public ItemLocation Unknown3 { get; private set; } = new ItemLocation();
    public ItemLocation Unknown4 { get; private set; } = new ItemLocation();
    [UInt(32)] public uint Unknown5 { get; private set; }
}
