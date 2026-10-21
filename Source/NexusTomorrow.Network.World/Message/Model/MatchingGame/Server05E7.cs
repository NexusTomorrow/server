using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.MatchingGame;

[WireMessage(GameMessageOpcode.Server05E7, Direction.ServerToClient)]
public sealed partial class Server05E7
{
    public interface IServer05E7Union0 : IWireStruct
    {
    }

    [WireStruct]
    [UnionCase(0)]
    public sealed partial class Server05E7Union0Arm00 : IServer05E7Union0
    {
        public RealmScopedId Unknown0 { get; set; } = new();
        [UInt(5)] public byte Unknown1 { get; set; }
    }

    [WireStruct]
    [UnionCase(1)]
    public sealed partial class Server05E7Union0Arm01 : IServer05E7Union0
    {
        [UInt(32)] public uint Unknown0 { get; set; }
        [UInt(18)] public uint Unknown1 { get; set; }
    }

    [UInt(2)] public byte Unknown0 { get; set; }
    [Union(nameof(Unknown0))] public IServer05E7Union0 Unknown1 { get; set; } = new Server05E7Union0Arm00();

    [UInt(2)] public byte Unknown2 { get; set; }
    [Union(nameof(Unknown2))] public IServer05E7Union0 Unknown3 { get; set; } = new Server05E7Union0Arm00();

    [UInt(2)] public byte Unknown4 { get; set; }
    [UInt(3)] public byte Unknown5 { get; set; }
}
