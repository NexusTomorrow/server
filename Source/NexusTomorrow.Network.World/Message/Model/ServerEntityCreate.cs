using NexusTomorrow.Game.Static.Entity;
using NexusTomorrow.Game.Static.Reputation;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Entity;
using NexusTomorrow.Network.World.Entity.Model;
using NexusTomorrow.Network.World.Message.Model.Shared;
using NexusTomorrow.Network.World.Message.Model.Spell;

namespace NexusTomorrow.Network.World.Message.Model;

[WireMessage(GameMessageOpcode.ServerEntityCreate, Direction.ServerToClient)]
public sealed partial class ServerEntityCreate
{
    #region Unknown Structures

    [WireStruct]
    public sealed partial class UnknownStructureA8
    {
        /// <summary>
        /// The payload <see cref="Type"/> selects; only cases 0 and 1 have a known layout.
        /// </summary>
        public interface IPayload : IWireStruct
        {
        }

        [WireStruct]
        [UnionCase(0)]
        public sealed partial class Flag : IPayload
        {
            [UInt(1)] public bool Unknown0 { get; set; }
        }

        [WireStruct]
        [UnionCase(1)]
        public sealed partial class Value : IPayload
        {
            [UInt(17)] public uint Unknown1 { get; set; }
        }

        [UInt(2)] public byte Type { get; set; }
        [Union(nameof(Type))] public IPayload Payload { get; set; } = new Flag();
    }

    [WireStruct]
    public sealed partial class WorldPlacement
    {
        /// <summary>
        /// The payload <see cref="Type"/> selects; only cases 0 and 1 have a known layout.
        /// </summary>
        public interface IPayload : IWireStruct
        {
        }

        [WireStruct]
        [UnionCase(0)]
        public sealed partial class Flag : IPayload
        {
            [UInt(1)] public bool Unknown0 { get; set; }
        }

        [WireStruct]
        [UnionCase(1)]
        public sealed partial class Socket : IPayload
        {
            [UInt(64)] public ulong ActivePropId { get; set; }
            [UInt(14)] public ushort SocketId { get; set; }
        }

        [UInt(2)] public byte Type { get; set; }
        [Union(nameof(Type))] public IPayload Payload { get; set; } = new Flag();
    }

    [WireStruct]
    public sealed partial class UnknownStructureC8
    {
        /// <summary>
        /// The payload <see cref="Type"/> selects; only cases 0 and 1 have a known layout.
        /// </summary>
        public interface IPayload : IWireStruct
        {
        }

        [WireStruct]
        [UnionCase(0)]
        public sealed partial class Flag : IPayload
        {
            [UInt(1)] public bool Unknown0 { get; set; }
        }

        [WireStruct]
        [UnionCase(1)]
        public sealed partial class Values : IPayload
        {
            [UInt(32)] public uint Unknown1 { get; set; }
            [UInt(18)] public uint Unknown2 { get; set; }
        }

        [UInt(2)] public byte Type { get; set; }
        [Union(nameof(Type))] public IPayload Payload { get; set; } = new Flag();
    }

    #endregion

    [UInt(32)] public uint Guid { get; set; }
    [UInt(6)] public EntityType Type { get; set; }
    [Union(nameof(Type))] public IEntityModel EntityModel { get; set; } = new NonPlayerEntityModel();
    [UInt(8)] public EntityCreateFlag CreateFlags { get; set; }
    [Array(CountBits = 5)] public List<StatValueInitial> Stats { get; set; } = [];
    [UInt(32)] public uint Time { get; set; }
    [Array(CountBits = 5)] public List<NetworkEntityCommand> Commands { get; set; } = [];
    [Array(CountBits = 8)] public List<PropertyValue> Properties { get; set; } = [];
    [Array(CountBits = 7)] public List<ItemVisual> VisibleItems { get; set; } = [];
    [Array(CountBits = 9)] public List<SpellInit> SpellInitData { get; } = [];
    [UInt(32)] public uint CurrentSpellUniqueId { get; set; }
    [UInt(14)] public Faction Faction1 { get; set; }
    [UInt(14)] public Faction Faction2 { get; set; }
    [UInt(32)] public uint UnitTagOwner { get; set; }
    [UInt(64)] public ulong GroupTagOwner { get; set; }
    public UnknownStructureA8 UnknownA8 { get; set; } = new();
    public WorldPlacement WorldPlacementData { get; set; } = new();
    public UnknownStructureC8 UnknownC8 { get; set; } = new();
    [UInt(14)] public ushort MiniMapMarker { get; set; }
    [UInt(17)] public uint DisplayInfo { get; set; }
    [UInt(15)] public ushort OutfitInfo { get; set; }
}
