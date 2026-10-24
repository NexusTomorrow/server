using System.Numerics;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Pregame;

/// <summary>
/// The account's characters and its character creation options, the answer to <see cref="CharacterListRequest"/>. The client
/// replaces its list with this one.
/// </summary>
/// <remarks>
/// Taken while character select is active.
/// </remarks>
[WireMessage(GameMessageOpcode.Server0117, Direction.ServerToClient)]
[Verified(Rung.Purpose)]
public sealed partial class CharacterList
{
    /// <summary>
    /// One of the account's characters.
    /// </summary>
    [WireStruct]
    public sealed partial class Character
    {
        /// <summary>
        /// The character's id, which the client sends back to select, delete or rename it.
        /// </summary>
        [VerifiedMember, UInt(64)] public ulong CharacterId { get; set; }

        /// <summary>
        /// The character's name.
        /// </summary>
        [VerifiedMember, WideString] public string Name { get; set; } = "";

        /// <summary>
        /// The character's gender id.
        /// </summary>
        [VerifiedMember, UInt(2)] public byte GenderId { get; set; }

        /// <summary>
        /// The character's race id.
        /// </summary>
        [VerifiedMember, UInt(5)] public byte RaceId { get; set; }

        /// <summary>
        /// The character's class id.
        /// </summary>
        [VerifiedMember, UInt(5)] public byte ClassId { get; set; }

        /// <summary>
        /// The client shows this value as the character's faction on character select.
        /// </summary>
        [VerifiedMember, UInt(32)] public uint Unknown5 { get; set; }

        /// <summary>
        /// The client shows this value as the character's level on character select.
        /// </summary>
        [VerifiedMember, UInt(32)] public uint Unknown6 { get; set; }

        /// <summary>
        /// Items that set a slot's look only: the client reads each item's slot and display id here, and neither colour field.
        /// </summary>
        [VerifiedMember, Array(CountBits = 32)] public List<ItemVisual> PlainItems { get; } = [];

        /// <summary>
        /// Items that set a slot's look and colour, applied after <see cref="PlainItems"/> and over it for the same slot.
        /// </summary>
        [VerifiedMember, Array(CountBits = 32)] public List<ItemVisual> ColoredItems { get; } = [];

        /// <summary>
        /// The world the character is in.
        /// </summary>
        [VerifiedMember, UInt(15)] public ushort WorldId { get; set; }

        /// <summary>
        /// A zone id the client uses, with <see cref="WorldId"/> and <see cref="Position"/>, to pick the zone whose name it hands to its
        /// interface.
        /// </summary>
        [VerifiedMember, UInt(15)] public ushort ZoneId { get; set; }

        /// <summary>
        /// The client gives this value no meaning.
        /// </summary>
        [VerifiedMember, UInt(14)] public ushort Unknown11 { get; set; }

        /// <summary>
        /// A point the client uses, x and z only, with <see cref="WorldId"/> and <see cref="ZoneId"/>, to pick the zone whose name it hands
        /// to its interface.
        /// </summary>
        [VerifiedMember, Float32] public Vector3 Position { get; set; }

        /// <summary>
        /// The client gives this value no meaning.
        /// </summary>
        [VerifiedMember, Float32] public float Unknown13 { get; set; }

        /// <summary>
        /// The client gives this value no meaning.
        /// </summary>
        [VerifiedMember, Float32] public float Unknown14 { get; set; }

        /// <summary>
        /// The client shows this value as the character's path on character select, counted from 0.
        /// </summary>
        [VerifiedMember, UInt(3)] public byte Unknown15 { get; set; }

        /// <summary>
        /// The client shows this flag as whether the character is disabled on character select.
        /// </summary>
        [VerifiedMember, UInt(1)] public bool Unknown16 { get; set; }

        /// <summary>
        /// The client shows this flag as whether the character must be renamed on character select.
        /// </summary>
        [VerifiedMember, UInt(1)] public bool Unknown17 { get; set; }

        /// <summary>
        /// Which costume entries the character shows: bit i set shows entry i of <see cref="CostumeItems"/>, clear empties its slot. Entries
        /// 0 to 5 are item slots 1 to 6 and entry 6 is slot 20.
        /// </summary>
        [VerifiedMember, UInt(32)] public uint CostumeMask { get; set; }

        /// <summary>
        /// How many costume entries follow; the client uses at most seven.
        /// </summary>
        [VerifiedMember, UInt(4), Count] public byte CostumeCount { get; set; }

        /// <summary>
        /// The costume's items, one per entry, as ids in the Item2 table.
        /// </summary>
        [VerifiedMember, Array(CountFrom = nameof(CostumeCount)), UInt(32)] public List<uint> CostumeItems { get; } = [];

        /// <summary>
        /// A colour per costume entry, laid over its item's own colour.
        /// </summary>
        [VerifiedMember, Array(CountFrom = nameof(CostumeCount)), UInt(32)] public List<uint> CostumeColors { get; } = [];

        /// <summary>
        /// Values the client applies to the character's model.
        /// </summary>
        [VerifiedMember, Array(CountBits = 32), Float32] public List<float> Bones { get; } = [];

        [Float32] public float Unknown23 { get; set; }
    }

    /// <summary>
    /// The server's clock, as a Windows file time. The client keeps its difference from its own clock.
    /// </summary>
    [VerifiedMember, UInt(64)] public ulong ServerTime { get; set; }

    /// <summary>
    /// The account's characters.
    /// </summary>
    [VerifiedMember, Array(CountBits = 32)] public List<Character> Characters { get; } = [];

    /// <summary>
    /// Character creation options to add to those the client considers, as ids in the CharacterCreation table.
    /// </summary>
    [VerifiedMember, Array(CountBits = 32), UInt(32)] public List<uint> OfferedCreationIds { get; set; } = [];

    /// <summary>
    /// Character creation options to withhold, as ids in the CharacterCreation table.
    /// </summary>
    [VerifiedMember, Array(CountBits = 32), UInt(32)] public List<uint> WithheldCreationIds { get; set; } = [];

    /// <summary>
    /// A realm id; the client takes it as its current realm when a character is selected.
    /// </summary>
    [VerifiedMember, UInt(14)] public ushort RealmId { get; set; }

    /// <summary>
    /// A listed character, by its realm and its id as in <see cref="Characters"/>, that the client hands to its interface when both parts
    /// are not 0 and the character is listed.
    /// </summary>
    [VerifiedMember] public RealmScopedId Removal { get; set; } = new();

    [UInt(32)] public uint Unknown6 { get; set; }
    [UInt(32)] public uint Unknown7 { get; set; }

    /// <summary>
    /// The client gives this value no meaning. It hands it to its interface with the list.
    /// </summary>
    [VerifiedMember, UInt(32)] public uint Unknown8 { get; set; }

    /// <summary>
    /// The client gives this value no meaning.
    /// </summary>
    [VerifiedMember, UInt(32)] public uint Unknown9 { get; set; }

    /// <summary>
    /// The faction the interface shows the account locked to: 166 Dominion, 167 Exile, 0 none. The client hands it to its interface, which
    /// applies it only on a realm that keeps characters to one faction.
    /// </summary>
    [VerifiedMember, UInt(14)] public ushort FactionRestriction { get; set; }

    /// <summary>
    /// Whether the account may create characters at level 50 for free: it gates the interface's count of those and lets character
    /// creation skip an entitlement check for one type of creation option.
    /// </summary>
    [VerifiedMember, UInt(1)] public bool FreeLevel50 { get; set; }
}
