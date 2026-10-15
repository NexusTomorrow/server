using NexusTomorrow.Game.Static.Entity;
using NexusTomorrow.Game.Static.Reputation;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Entity;
using NexusTomorrow.Network.World.Entity.Model;
using NexusTomorrow.Network.World.Message.Model.Shared;
using NexusTomorrow.Network.World.Message.Model.Spell;

namespace NexusTomorrow.Network.World.Message.Model;

[Message(GameMessageOpcode.ServerEntityCreate)]
[WireMessage(GameMessageOpcode.ServerEntityCreate, Direction.ServerToClient)]
public sealed partial class ServerEntityCreate : IWritable
{
    #region Unknown Structures

    [WireStruct]
    public sealed partial class UnknownStructureA8 : IWritable
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
        [NotOnWire] public bool Unknown0 { get; set; }
        [NotOnWire] public uint Unknown1 { get; set; }

        public void Write(GamePacketWriter writer)
        {
            writer.Write(Type, 2);

            switch (Type)
            {
                case 0:
                    writer.Write(Unknown0);
                    break;
                case 1:
                    writer.Write(Unknown1, 17u);
                    break;
            }
        }
    }

    [WireStruct]
    public sealed partial class WorldPlacement : IWritable
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
        [NotOnWire] public bool Unknown0 { get; set; }
        [NotOnWire] public ulong ActivePropId { get; set; }
        [NotOnWire] public ushort SocketId { get; set; }

        public void Write(GamePacketWriter writer)
        {
            writer.Write(Type, 2u);

            switch (Type)
            {
                case 0:
                    writer.Write(Unknown0);
                    break;
                case 1:
                    writer.Write(ActivePropId);
                    writer.Write(SocketId, 14u);
                    break;
            }
        }
    }

    [WireStruct]
    public sealed partial class UnknownStructureC8 : IWritable
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
        [NotOnWire] public bool Unknown0 { get; set; }
        [NotOnWire] public uint Unknown1 { get; set; }
        [NotOnWire] public uint Unknown2 { get; set; }

        public void Write(GamePacketWriter writer)
        {
            writer.Write(Type, 2);

            switch (Type)
            {
                case 0:
                    writer.Write(Unknown0);
                    break;
                case 1:
                    writer.Write(Unknown1);
                    writer.Write(Unknown2, 18);
                    break;
            }
        }
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

    public void Write(GamePacketWriter writer)
    {
        writer.Write(Guid);
        writer.Write(Type, 6);
        EntityModel.Write(writer);
        writer.Write(CreateFlags, 8);

        writer.Write((byte)Stats.Count, 5);
        Stats.ForEach(o => o.Write(writer));

        writer.Write(Time);

        writer.Write((byte)Commands.Count, 5);
        foreach (INetworkEntityCommand command in Commands)
        {
            writer.Write(command.Command, 5);
            command.Model.Write(writer);
        }

        writer.Write((byte)Properties.Count);
        Properties.ForEach(o => o.Write(writer));

        writer.Write((byte)VisibleItems.Count, 7);
        VisibleItems.ForEach(o => o.Write(writer));

        writer.Write((short)SpellInitData.Count, 9);
        SpellInitData.ForEach(o => o.Write(writer));

        writer.Write(CurrentSpellUniqueId);
        writer.Write(Faction1, 14);
        writer.Write(Faction2, 14);
        writer.Write(UnitTagOwner);
        writer.Write(GroupTagOwner);

        UnknownA8.Write(writer);
        WorldPlacementData.Write(writer);
        UnknownC8.Write(writer);

        writer.Write(MiniMapMarker, 14);
        writer.Write(DisplayInfo, 17);
        writer.Write(OutfitInfo, 15);
    }
}
