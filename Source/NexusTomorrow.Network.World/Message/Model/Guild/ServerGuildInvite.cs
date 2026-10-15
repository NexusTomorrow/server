using NexusTomorrow.Game.Static.Guild;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Guild;

[WireMessage(GameMessageOpcode.ServerGuildInvite, Direction.ServerToClient)]
public sealed partial class ServerGuildInvite
{
    [WideString] public string InviterName { get; set; }
    [WideString] public string GuildName { get; set; }
    [UInt(32)] public GuildFlag Flags { get; set; }
    [UInt(4)] public GuildType GuildType { get; set; }
    [UInt(64)] public ulong Unused { get; set; }
}
