using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.PlayerPath;

[WireMessage(GameMessageOpcode.Client06B2, Direction.ClientToServer)]
public sealed partial class PathChangeRequest
{
    [UInt(3)] public byte Unknown0 { get; private set; }
    [UInt(1)] public bool Unknown1 { get; private set; }
}
