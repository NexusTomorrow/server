using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Store;

[WireMessage(GameMessageOpcode.Client082A, Direction.ClientToServer)]
public sealed partial class Client082A
{
    [UInt(20)] public uint Unknown0 { get; set; }
    [UInt(5)] public byte Unknown1 { get; set; }
    [UInt(14)] public ushort Unknown2 { get; set; }
    [UInt(20)] public uint Unknown3 { get; set; }
    public RealmScopedId Unknown4 { get; set; } = new();
    [UInt(20)] public uint Unknown5 { get; set; }
}
