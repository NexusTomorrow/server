using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.Message.Model;

[WireMessage(GameMessageOpcode.ClientEncrypted, Direction.ClientToServer)]
public sealed partial class ClientEncrypted
{
    [Blob(32, LengthIncludesSelf = true)] public byte[] Data { get; private set; } = [];
}
