using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Guild;

[WireMessage(GameMessageOpcode.Client04A6, Direction.ClientToServer)]
public sealed partial class GuildModifyHolomark
{
    public RealmScopedId Unknown0 { get; private set; } = new();
    public GuildHolomark Unknown1 { get; private set; } = new();
}
