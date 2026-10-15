using NexusTomorrow.Game.Static.Housing;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Housing;

[WireMessage(GameMessageOpcode.ServerHousingResidences, Direction.ServerToClient)]
public sealed partial class ServerHousingResidences
{
    [WireStruct]
    public sealed partial class Residence
    {
        public Identity ResidenceIdentity { get; set; }
        [UInt(64)] public ulong NeighbourhoodId { get; set; }
        [UInt(64)] public ulong CharacterIdOwner { get; set; } // 0 when no character owns the residence
        [UInt(64)] public ulong GuildIdOwner { get; set; } // 0 when no guild owns the residence
        [UInt(14)] public ResidenceType Type { get; set; }
        [UInt(32)] public uint TileId { get; set; }
        [WideString] public string Name { get; set; }
        [UInt(32)] public PropertyInfoId PropertyInfoId { get; set; }
        [UInt(32)] public uint ResidenceInfoId { get; set; }
        [UInt(32)] public uint WallpaperExterior { get; set; }
        [UInt(32)] public uint Entryway { get; set; }
        [UInt(32)] public uint Roof { get; set; }
        [UInt(32)] public uint Door { get; set; }
        [UInt(32)] public uint Sky { get; set; }
        [UInt(32)] public uint Music { get; set; }
        [UInt(32)] public uint Ground { get; set; }
        [UInt(32)] public ResidenceFlags Flags { get; set; }
        [UInt(32)] public uint NeighbourHarvestSplit { get; set; }
        [UInt(32)] public uint NeighbourGardenSplit { get; set; }
        [Pad(512)] public Pad Unknown { get; } // 64 bytes of unknown meaning, sent as zeros
        [UInt(1)] public bool ResidenceDeleted { get; set; }
    }

    [Array(CountBits = 32)] public List<Residence> Residences { get; } = [];
}
