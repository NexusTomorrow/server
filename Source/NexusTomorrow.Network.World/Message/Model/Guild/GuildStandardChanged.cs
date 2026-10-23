using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Guild;

[WireMessage(GameMessageOpcode.Server04D0, Direction.ServerToClient)]
public sealed partial class GuildStandardChanged
{
    public RealmScopedId Unknown0 { get; set; } = new();
    public GuildHolomark Unknown1 { get; set; } = new();
}
