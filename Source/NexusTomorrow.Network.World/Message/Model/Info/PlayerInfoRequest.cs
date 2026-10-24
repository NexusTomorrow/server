using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Info;

[WireMessage(GameMessageOpcode.Client0597, Direction.ClientToServer)]
public sealed partial class PlayerInfoRequest
{
    [UInt(4)] public byte Unknown0 { get; private set; }
    public RealmScopedId Unknown1 { get; } = new();
}
