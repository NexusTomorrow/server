using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Guild;

[WireMessage(GameMessageOpcode.ServerRecruitmentGuildMinLevel, Direction.ServerToClient)]
public sealed partial class ServerRecruitmentGuildMinLevel
{
    public Identity GuildIdentity { get; set; }
    [UInt(32)] public uint MinimumLevel { get; set; }
}
