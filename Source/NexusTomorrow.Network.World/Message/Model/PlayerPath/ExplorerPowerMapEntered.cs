using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.PlayerPath;

[WireMessage(GameMessageOpcode.Server00F6, Direction.ServerToClient)]
public sealed partial class ExplorerPowerMapEntered
{
    [UInt(14)] public ushort Unknown0 { get; set; }
}
