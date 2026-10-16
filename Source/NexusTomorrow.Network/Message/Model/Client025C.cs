using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.Message.Model;

[WireMessage(GameMessageOpcode.Client025C, Direction.ClientToServer)]
public sealed partial class Client025C
{
    [Blob(32, LengthIncludesSelf = true)] public byte[] Unknown0 { get; private set; } = [];
}
