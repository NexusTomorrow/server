using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Guild;

/// <summary>
/// Sets the guild a unit shows on its nameplate.
/// </summary>
/// <remarks>
/// The client raises UnitGuildNameplateChanged.
/// </remarks>
[WireMessage(GameMessageOpcode.Server04AE, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class UnitGuildNameplateChanged
{
    /// <summary>
    /// The unit, by unit id.
    /// </summary>
    [UInt(32)] public uint UnitId { get; set; }

    /// <summary>
    /// The guild's name, which Unit:GetGuildName returns.
    /// </summary>
    [WideString] public string GuildName { get; set; } = "";

    /// <summary>
    /// The kind of guild, a GuildType value, which Unit:GetGuildType returns.
    /// </summary>
    [UInt(4)] public byte GuildType { get; set; }
}
