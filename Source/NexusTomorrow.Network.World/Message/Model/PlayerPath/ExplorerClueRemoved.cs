using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.PlayerPath;

[WireMessage(GameMessageOpcode.Server06B7, Direction.ServerToClient)]
public sealed partial class ExplorerClueRemoved
{
    [UInt(14)] public ushort Unknown0 { get; set; }
}
