using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.Message.Model;

[WireMessage(GameMessageOpcode.Client0244, Direction.ClientToServer)]
public sealed partial class Client0244
{
    [Blob(32, LengthIncludesSelf = true)] public byte[] Unknown0 { get; private set; } = [];
}
