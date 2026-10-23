using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Guild;

[WireMessage(GameMessageOpcode.Server049F, Direction.ServerToClient)]
public sealed partial class GuildRecruitmentAvailabilityChanged
{
    public RealmScopedId Unknown0 { get; set; } = new();
    public RealmScopedId Unknown1 { get; set; } = new();
    [UInt(32)] public uint Unknown2 { get; set; }
}
