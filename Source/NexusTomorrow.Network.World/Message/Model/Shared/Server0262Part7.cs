using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Message.Model.Shared;

[WireStruct]
public sealed partial class Server0262Part7
{
    /// <summary>
    /// The spell's caster, by unit id.
    /// </summary>
    [VerifiedMember, UInt(32)] public uint CasterId { get; set; }

    /// <summary>
    /// The spell's target, by unit id.
    /// </summary>
    [VerifiedMember, UInt(32)] public uint TargetId { get; set; }

    /// <summary>
    /// The cast, by the server's id for it. When it is not 0 and the client already holds a spell under it, the client refreshes that
    /// spell; otherwise it files the new spell under this id.
    /// </summary>
    [VerifiedMember, UInt(32)] public uint CastId { get; set; }

    /// <summary>
    /// The spell, as an id in the client's Spell4 table.
    /// </summary>
    [VerifiedMember, UInt(18)] public uint Spell4Id { get; set; }

    [UInt(1)] public bool Unknown4 { get; set; }

    /// <summary>
    /// The spell's targets, each a unit and the effects on it.
    /// </summary>
    [VerifiedMember, Array(CountBits = 32)] public List<Server0262Part8> Targets { get; set; } = [];

    [Array(CountBits = 8)] public List<Server0262Part12> Unknown6 { get; set; } = [];
    [Array(CountBits = 8)] public List<Server0262Part13> Unknown7 { get; set; } = [];
}
