using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Entity.Model;

/// <summary>
/// The data of a unit of type 23.
/// </summary>
[WireStruct]
[UnionCase(23)]
public sealed partial class Server0262Union0Arm17 : IServer0262Union0
{
    /// <summary>
    /// The client gives this value no meaning.
    /// </summary>
    [VerifiedMember, UInt(64)] public ulong Unknown0 { get; set; }

    /// <summary>
    /// The client gives this value no meaning.
    /// </summary>
    [VerifiedMember, UInt(14)] public ushort Unknown1 { get; set; }

    /// <summary>
    /// The unit's name.
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
    /// The unit's gender id.
    /// </summary>
    [VerifiedMember, UInt(2)] public byte Gender { get; set; }

    /// <summary>
    /// The unit's group, by id; 0 when it is in none.
    /// </summary>
    [VerifiedMember, UInt(64)] public ulong GroupId { get; set; }

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
    [VerifiedMember, Array(CountBits = 6), Float32] public List<float> BoneValues { get; set; } = [];

    /// <summary>
    /// The unit's title, a CharacterTitle table id, which Unit:HasTitle and Unit:GetTitleOrName read;
    /// <see cref="Message.Model.Entity.UnitNameTitleChanged"/> sets it too. The client keeps the title only when the unit's name is not
    /// empty; with an empty name it also clears the title's text.
    /// </summary>
    [VerifiedMember, UInt(14)] public ushort TitleId { get; set; }
}
