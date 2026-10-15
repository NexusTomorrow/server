using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

[WireMessage(GameMessageOpcode.ClientReplayLevelRequest, Direction.ClientToServer)]
public sealed partial class ClientReplayLevelUp
{
    [UInt(32)] public uint Level { get; private set; }
}
