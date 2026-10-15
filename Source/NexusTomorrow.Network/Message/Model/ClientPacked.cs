using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.Message.Model;

[WireMessage(GameMessageOpcode.ClientPacked, Direction.ClientToServer)]
public sealed partial class ClientPacked
{
    [Blob(32, LengthIncludesSelf = true)] public byte[] Data { get; private set; } = [];
}
