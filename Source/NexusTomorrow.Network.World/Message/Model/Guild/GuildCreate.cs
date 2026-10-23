using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Guild;

/// <summary>
/// Creates a guild: the request GuildLib.Create sends, among others.
/// </summary>
/// <remarks>
/// Buying a community sends type 7 with the default rank names and no holomark.
/// </remarks>
[WireMessage(GameMessageOpcode.Client0481, Direction.ClientToServer)]
[Verified(Rung.Fields)]
public sealed partial class GuildCreate
{
    /// <summary>
    /// The registrar the player is talking to, by unit id; 0 when the player is not at the registrar for this kind of guild.
    /// </summary>
    [UInt(32)] public uint RegistrarUnitId { get; private set; }

    /// <summary>
    /// The kind of guild, a GuildType value.
    /// </summary>
    [UInt(4)] public byte Type { get; private set; }

    /// <summary>
    /// The guild's name.
    /// </summary>
    [WideString] public string Name { get; private set; } = "";

    /// <summary>
    /// The name of the guild master's rank.
    /// </summary>
    [WideString] public string MasterRankName { get; private set; } = "";

    /// <summary>
    /// The name of the council's rank.
    /// </summary>
    [WideString] public string CouncilRankName { get; private set; } = "";

    /// <summary>
    /// The name of the members' rank.
    /// </summary>
    [WideString] public string MemberRankName { get; private set; } = "";

    /// <summary>
    /// The guild's holomark; empty when the sender gives none.
    /// </summary>
    public GuildHolomark Holomark { get; private set; } = new();

    /// <summary>
    /// True to pay the alternate cost: buying a community with service tokens.
    /// </summary>
    [UInt(1)] public bool UseAlternateCost { get; private set; }
}
