using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Guild;

[WireMessage(GameMessageOpcode.Server04A2, Direction.ServerToClient)]
public sealed partial class GuildMemberRemove
{
    public RealmScopedId Unknown0 { get; set; } = new();
    public RealmScopedId Unknown1 { get; set; } = new();
    [UInt(16)] public ushort Unknown2 { get; set; }
    [UInt(16)] public ushort Unknown3 { get; set; }
}
