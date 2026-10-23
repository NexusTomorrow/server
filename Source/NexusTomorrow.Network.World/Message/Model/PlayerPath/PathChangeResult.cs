using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.PlayerPath;

[WireMessage(GameMessageOpcode.Server06B3, Direction.ServerToClient)]
public sealed partial class PathChangeResult
{
    [UInt(8)] public byte Unknown0 { get; set; }
}
