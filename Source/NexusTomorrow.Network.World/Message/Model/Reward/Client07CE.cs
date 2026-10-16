using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Reward;

[WireMessage(GameMessageOpcode.Client07CE, Direction.ClientToServer)]
public sealed partial class Client07CE
{
    [UInt(14)] public ushort Unknown0 { get; private set; }
    [UInt(32)] public uint Unknown1 { get; private set; }
}
