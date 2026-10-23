using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.PlayerPath;

[WireMessage(GameMessageOpcode.Server00FA, Direction.ServerToClient)]
public sealed partial class ExplorerPowerMapWaiting
{
    [UInt(14)] public ushort Unknown0 { get; set; }
}
