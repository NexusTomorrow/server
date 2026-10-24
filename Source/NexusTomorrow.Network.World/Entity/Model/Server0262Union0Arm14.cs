using NexusTomorrow.Game.Static.Entity;
using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Entity.Model;

/// <summary>
/// The data of a unit of type 20.
/// </summary>
[WireStruct]
[UnionCase(20)]
public sealed partial class Server0262Union0Arm14 : IServer0262Union0
{
    /// <summary>
    /// The character's id, as <see cref="Message.Model.Pregame.CharacterList.Character.CharacterId"/> gives it. With
    /// <see cref="RealmId"/> it makes a realm-scoped id: the key under which the client files the unit's name, and the unit's id when that
    /// id's top two bits are clear. Some of the client's name requests look the name up under that key before they ask the server.
    /// GroupLib.GetMentoringList finds a group member's unit through it, and GameLib.GetPlayerUnitByName reads the same map. The client
    /// refuses the unit when it is 0.
    /// </summary>
    [VerifiedMember, UInt(64)] public ulong CharacterId { get; set; }

    /// <summary>
    /// The realm <see cref="CharacterId"/> belongs to. With it, the key under which the client files the unit's name, and the unit's id
    /// when that id's top two bits are clear. The client refuses the unit when it is 0.
    /// </summary>
    [VerifiedMember, UInt(14)] public ushort RealmId { get; set; }

    /// <summary>
    /// The unit's name. When it is empty the client uses a default name.
    /// </summary>
    [VerifiedMember, WideString] public string Name { get; set; } = "";

    /// <summary>
    /// The unit's race id. The client refuses the unit when it is above 17.
    /// </summary>
    [VerifiedMember, UInt(5)] public byte RaceId { get; set; }

    /// <summary>
    /// The unit's class, as an id in the client's Class table. An id the table lacks does not refuse the unit.
    /// </summary>
    [VerifiedMember, UInt(5)] public byte ClassId { get; set; }

    /// <summary>
    /// The unit's gender.
    /// </summary>
    [VerifiedMember, UInt(2)] public Sex Gender { get; set; }

    /// <summary>
    /// The unit's group, by id; 0 when it is in none. Unit:IsInYourGroup compares it with the player's current group, and
    /// <see cref="Message.Model.Group.UnitGroupChanged"/> sets it.
    /// </summary>
    [VerifiedMember, UInt(64)] public ulong GroupId { get; set; }

    [Array(CountBits = 8), UInt(32)] public List<uint> Unknown7 { get; } = [];

    /// <summary>
    /// The name of the unit's guild, which Unit:GetGuildName and Unit:GetAffiliationName read.
    /// </summary>
    [VerifiedMember, WideString] public string GuildName { get; set; } = "";

    /// <summary>
    /// The kind of guild, a GuildType value, which Unit:GetGuildType returns; <see cref="Message.Model.Guild.UnitGuildNameplateChanged"/>
    /// sets it too.
    /// </summary>
    [VerifiedMember, UInt(4)] public byte GuildType { get; set; }

    /// <summary>
    /// The unit's guilds, by their ids within the realm, which GuildTypeLib.IsUnitMember reads;
    /// <see cref="Message.Model.Entity.UnitGuildsChanged"/> replaces them.
    /// </summary>
    [VerifiedMember, Array(CountBits = 5), UInt(64)] public List<ulong> GuildIds { get; set; } = [];

    /// <summary>
    /// The unit's bone values, which the client applies to the unit's model with the bone set of its race and gender.
    /// </summary>
    [VerifiedMember, Array(CountBits = 6), Float32] public List<float> ModelValues { get; set; } = [];

    /// <summary>
    /// The unit's PvP flags, of which Unit:IsPvpFlagged reads bit 0.
    /// </summary>
    [VerifiedMember, UInt(3)] public byte PvpFlags { get; set; }

    /// <summary>
    /// The unit's premium tier, with 255 read as 0. Unit:GetPremiumTier returns it, and the client puts it on the unit's chat lines and
    /// emotes.
    /// </summary>
    [VerifiedMember, UInt(8)] public byte PremiumTier { get; set; }

    /// <summary>
    /// The unit's title, a CharacterTitle table id, which Unit:HasTitle and Unit:GetTitleOrName read;
    /// <see cref="Message.Model.Entity.UnitNameTitleChanged"/> sets it too. The client keeps the title only when the unit's name is not
    /// empty; with an empty name it also clears the title's text.
    /// </summary>
    [VerifiedMember, UInt(14)] public ushort TitleId { get; set; }
}
