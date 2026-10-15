using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Guild;

[WireMessage(GameMessageOpcode.ServerGuildJoin, Direction.ServerToClient)]
public sealed partial class ServerGuildJoin
{
    public GuildData GuildData { get; set; } = new();
    public GuildMember Self { get; set; } = new();
    public GuildWithdrawlInfo WithdrawlInfo { get; set; } = new();
    [UInt(1)] public bool DisplayThisGuildNameplate { get; set; } = true;
}
