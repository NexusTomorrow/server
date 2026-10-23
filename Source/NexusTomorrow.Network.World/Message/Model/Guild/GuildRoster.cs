using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Guild;

[WireMessage(GameMessageOpcode.Server04A0, Direction.ServerToClient)]
public sealed partial class GuildRoster
{
    public RealmScopedId Unknown0 { get; set; } = new();
    [Array(CountBits = 32)] public List<GuildMember> Unknown1 { get; set; } = [];
    [UInt(1)] public bool Unknown2 { get; set; }
}
