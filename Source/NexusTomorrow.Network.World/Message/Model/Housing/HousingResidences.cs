using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Housing;

/// <summary>
/// Updates the residences the client holds: it adds or refreshes each residence listed, or removes it when the record says so.
/// </summary>
[WireMessage(GameMessageOpcode.Server04DF, Direction.ServerToClient)]
[Verified(Rung.Purpose)]
public sealed partial class HousingResidences
{
    /// <summary>
    /// One residence.
    /// </summary>
    [WireStruct]
    public sealed partial class Residence
    {
        /// <summary>
        /// The residence, by id.
        /// </summary>
        public RealmScopedId Id { get; set; } = new();

        [UInt(64)] public ulong Unknown1 { get; set; }

        /// <summary>
        /// The residence's owner, by character id.
        /// </summary>
        [UInt(64)] public ulong OwnerId { get; set; }

        /// <summary>
        /// The guild that owns it, by id: the player's war party for a war plot, the community's guild for a community.
        /// </summary>
        [UInt(64)] public ulong OwnerGuildId { get; set; }

        /// <summary>
        /// What kind of residence it is; 5 is a community.
        /// </summary>
        [UInt(14)] public ushort Kind { get; set; }

        [UInt(32)] public uint Unknown5 { get; set; }

        /// <summary>
        /// The residence's name, which Residence:GetPropertyName returns.
        /// </summary>
        [WideString] public string PropertyName { get; set; } = "";

        /// <summary>
        /// The residence's property, a HousingPropertyInfo table id.
        /// </summary>
        [UInt(32)] public uint PropertyInfoId { get; set; }

        [UInt(32)] public uint Unknown8 { get; set; }
        [UInt(32)] public uint Unknown9 { get; set; }
        [UInt(32)] public uint Unknown10 { get; set; }
        [UInt(32)] public uint Unknown11 { get; set; }
        [UInt(32)] public uint Unknown12 { get; set; }
        [UInt(32)] public uint Unknown13 { get; set; }
        [UInt(32)] public uint Unknown14 { get; set; }
        [UInt(32)] public uint Unknown15 { get; set; }

        /// <summary>
        /// The residence's flags, by bit as <see cref="HousingSetResidenceOptions.Flags"/>.
        /// </summary>
        [UInt(32)] public uint Flags { get; set; }

        /// <summary>
        /// How its harvest is split with neighbors, as <see cref="HousingSetResidenceOptions.HarvestSplit"/>.
        /// </summary>
        [UInt(32)] public uint HarvestSplit { get; set; }

        /// <summary>
        /// How its garden is split with neighbors, as <see cref="HousingSetResidenceOptions.GardenSplit"/>.
        /// </summary>
        [UInt(32)] public uint GardenSplit { get; set; }

        [FixedArray(8), UInt(64)] public ulong[] Unknown19 { get; set; } = new ulong[8];

        /// <summary>
        /// True when the client is to remove the residence.
        /// </summary>
        [UInt(1)] public bool Removed { get; set; }
    }

    /// <summary>
    /// The residences.
    /// </summary>
    [Array(CountBits = 32)] public List<Residence> Residences { get; } = [];
}
