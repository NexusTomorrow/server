using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Guild;

[WireMessage(GameMessageOpcode.Server0493, Direction.ServerToClient)]
public sealed partial class GuildJoined
{
    public GuildRecord Unknown0 { get; set; } = new();
    public GuildMember Unknown1 { get; set; } = new();
    public GuildBankWithdrawals Unknown2 { get; set; } = new();
    [UInt(1)] public bool Unknown3 { get; set; }
}
