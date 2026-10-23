using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.PlayerPath;

[WireMessage(GameMessageOpcode.Server00F4, Direction.ServerToClient)]
public sealed partial class ExplorerPowerMapAdvanced
{
    [UInt(14)] public ushort Unknown0 { get; set; }
}
