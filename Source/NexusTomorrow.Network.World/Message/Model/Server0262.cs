using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Entity;
using NexusTomorrow.Network.World.Entity.Model;
using NexusTomorrow.Network.World.Message.Model.Shared;
using NexusTomorrow.Network.World.Message.Model.Spell;

namespace NexusTomorrow.Network.World.Message.Model;

[WireMessage(GameMessageOpcode.Server0262, Direction.ServerToClient)]
public sealed partial class Server0262
{
    #region Unknown Structures

    [WireStruct]
    public sealed partial class Server0262Part14
    {
        public interface IServer0262Union4 : IWireStruct
        {
        }

        [WireStruct]
        [UnionCase(0)]
        public sealed partial class Server0262Union4Arm00 : IServer0262Union4
        {
            [UInt(1)] public bool Unknown0 { get; set; }
        }

        [WireStruct]
        [UnionCase(1)]
        public sealed partial class Server0262Union4Arm01 : IServer0262Union4
        {
            [UInt(17)] public uint Unknown0 { get; set; }
        }

        [UInt(2)] public byte Unknown0 { get; set; }
        [Union(nameof(Unknown0))] public IServer0262Union4 Unknown1 { get; set; } = new Server0262Union4Arm00();
    }

    [WireStruct]
    public sealed partial class Server0262Part15
    {
        public interface IServer0262Union5 : IWireStruct
        {
        }

        [WireStruct]
        [UnionCase(0)]
        public sealed partial class Server0262Union5Arm00 : IServer0262Union5
        {
            [UInt(1)] public bool Unknown0 { get; set; }
        }

        [WireStruct]
        [UnionCase(1)]
        public sealed partial class Server0262Union5Arm01 : IServer0262Union5
        {
            [UInt(64)] public ulong Unknown0 { get; set; }
            [UInt(14)] public ushort Unknown1 { get; set; }
        }

        [UInt(2)] public byte Unknown0 { get; set; }
        [Union(nameof(Unknown0))] public IServer0262Union5 Unknown1 { get; set; } = new Server0262Union5Arm00();
    }

    [WireStruct]
    public sealed partial class Server0262Part16
    {
        public interface IServer0262Union6 : IWireStruct
        {
        }

        [WireStruct]
        [UnionCase(0)]
        public sealed partial class Server0262Union6Arm00 : IServer0262Union6
        {
            [UInt(1)] public bool Unknown0 { get; set; }
        }

        [WireStruct]
        [UnionCase(1)]
        public sealed partial class Server0262Union6Arm01 : IServer0262Union6
        {
            [UInt(32)] public uint Unknown0 { get; set; }
            [UInt(18)] public uint Unknown1 { get; set; }
        }

        [UInt(2)] public byte Unknown0 { get; set; }
        [Union(nameof(Unknown0))] public IServer0262Union6 Unknown1 { get; set; } = new Server0262Union6Arm00();
    }

    #endregion

    [UInt(32)] public uint Unknown0 { get; set; }
    [UInt(6)] public byte Unknown1 { get; set; }
    [Union(nameof(Unknown1))] public IServer0262Union0 Unknown2 { get; set; } = new Server0262Union0Arm00();
    [UInt(8)] public byte Unknown3 { get; set; }
    [Array(CountBits = 5)] public List<Server0262Part4> Unknown4 { get; set; } = [];
    [UInt(32)] public uint Unknown5 { get; set; }
    [Array(CountBits = 5)] public List<Server0262Part5> Unknown6 { get; set; } = [];
    [Array(CountBits = 8)] public List<Server0262Part6> Unknown7 { get; set; } = [];
    [Array(CountBits = 7)] public List<Server0117Part1> Unknown8 { get; set; } = [];
    [Array(CountBits = 9)] public List<Server0262Part7> Unknown9 { get; } = [];
    [UInt(32)] public uint Unknown10 { get; set; }
    [UInt(14)] public ushort Unknown11 { get; set; }
    [UInt(14)] public ushort Unknown12 { get; set; }
    [UInt(32)] public uint Unknown13 { get; set; }
    [UInt(64)] public ulong Unknown14 { get; set; }
    public Server0262Part14 Unknown15 { get; set; } = new();
    public Server0262Part15 Unknown16 { get; set; } = new();
    public Server0262Part16 Unknown17 { get; set; } = new();
    [UInt(14)] public ushort Unknown18 { get; set; }
    [UInt(17)] public uint Unknown19 { get; set; }
    [UInt(15)] public ushort Unknown20 { get; set; }
}
