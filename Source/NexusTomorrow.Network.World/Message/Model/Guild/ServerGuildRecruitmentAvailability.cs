using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Guild;

[WireMessage(GameMessageOpcode.ServerGuildRecruitmentAvailability, Direction.ServerToClient)]
public sealed partial class ServerGuildRecruitmentAvailability
{
    public Identity GuildIdentity1 { get; set; }
    public Identity GuildIdentity2 { get; set; } // GuildIdentity2 needs to match GuildIdentity1 for the handler to set the recruitment availability
    [UInt(32)] public uint RecruitmentAvailability { get; set; } // Bit 2 is the only bit used to signal recruitment availability
}
