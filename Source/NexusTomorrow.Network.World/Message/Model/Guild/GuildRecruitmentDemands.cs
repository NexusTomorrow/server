using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Message.Model.Guild;

/// <summary>
/// What a recruiting guild asks for: a value for each class in each role, as GuildTypeLib's GetRecruitmentDemands reports them.
/// </summary>
[WireStruct]
public sealed partial class GuildRecruitmentDemands
{
    /// <summary>
    /// The guild's demand for assault warriors.
    /// </summary>
    [UInt(8)] public byte WarriorAssault { get; set; }

    /// <summary>
    /// The guild's demand for support warriors.
    /// </summary>
    [UInt(8)] public byte WarriorSupport { get; set; }

    /// <summary>
    /// The guild's demand for assault engineers.
    /// </summary>
    [UInt(8)] public byte EngineerAssault { get; set; }

    /// <summary>
    /// The guild's demand for support engineers.
    /// </summary>
    [UInt(8)] public byte EngineerSupport { get; set; }

    /// <summary>
    /// The guild's demand for assault espers.
    /// </summary>
    [UInt(8)] public byte EsperAssault { get; set; }

    /// <summary>
    /// The guild's demand for support espers.
    /// </summary>
    [UInt(8)] public byte EsperSupport { get; set; }

    /// <summary>
    /// The guild's demand for assault medics.
    /// </summary>
    [UInt(8)] public byte MedicAssault { get; set; }

    /// <summary>
    /// The guild's demand for support medics.
    /// </summary>
    [UInt(8)] public byte MedicSupport { get; set; }

    /// <summary>
    /// The guild's demand for assault stalkers.
    /// </summary>
    [UInt(8)] public byte StalkerAssault { get; set; }

    /// <summary>
    /// The guild's demand for support stalkers.
    /// </summary>
    [UInt(8)] public byte StalkerSupport { get; set; }

    /// <summary>
    /// The guild's demand for assault spellslingers.
    /// </summary>
    [UInt(8)] public byte SpellslingerAssault { get; set; }

    /// <summary>
    /// The guild's demand for support spellslingers.
    /// </summary>
    [UInt(8)] public byte SpellslingerSupport { get; set; }
}
