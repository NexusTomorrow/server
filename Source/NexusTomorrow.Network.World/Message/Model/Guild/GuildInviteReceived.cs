using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Guild;

/// <summary>
/// Invites the player to a guild.
/// </summary>
/// <remarks>
/// The client raises GuildInvite with the guild's name, the inviter's, the type and the flags.
/// </remarks>
[WireMessage(GameMessageOpcode.Server0491, Direction.ServerToClient)]
[Verified(Rung.Purpose)]
public sealed partial class GuildInviteReceived
{
    /// <summary>
    /// The player who sent the invitation, by name.
    /// </summary>
    [WideString] public string InviterName { get; set; } = "";

    /// <summary>
    /// The guild's name.
    /// </summary>
    [WideString] public string GuildName { get; set; } = "";

    /// <summary>
    /// The guild's flags, a bit each as GuildTypeLib's SetFlags sets them.
    /// </summary>
    [UInt(32)] public uint GuildFlags { get; set; }

    /// <summary>
    /// The kind of guild, a GuildType value.
    /// </summary>
    [UInt(4)] public byte GuildType { get; set; }

    [UInt(64)] public ulong Unknown4 { get; set; }
}
