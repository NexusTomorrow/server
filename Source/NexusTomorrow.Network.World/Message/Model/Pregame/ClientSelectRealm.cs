using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Pregame;

[WireMessage(GameMessageOpcode.ClientSelectRealm, Direction.ClientToServer)]
public sealed partial class ClientSelectRealm
{
    [UInt(32)] public uint RealmId { get; private set; }
}
