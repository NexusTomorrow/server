using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.PlayerPath;

[WireMessage(GameMessageOpcode.Client00F9, Direction.ClientToServer)]
public sealed partial class ExplorerPowerMapProgress
{
    [UInt(14)] public uint Unknown0 { get; private set; }
}
