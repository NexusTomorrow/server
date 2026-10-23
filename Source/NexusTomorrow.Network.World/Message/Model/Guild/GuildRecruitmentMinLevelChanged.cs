using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Guild;

[WireMessage(GameMessageOpcode.Server076C, Direction.ServerToClient)]
public sealed partial class GuildRecruitmentMinLevelChanged
{
    public RealmScopedId Unknown0 { get; set; } = new();
    [UInt(32)] public uint Unknown1 { get; set; }
}
