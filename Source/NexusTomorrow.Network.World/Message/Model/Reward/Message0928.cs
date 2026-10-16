using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Reward;

[WireMessage(GameMessageOpcode.Message0928, Direction.Both)]
public sealed partial class Message0928
{
    [UInt(32)] public uint Unknown0 { get; set; }
    [UInt(5)] public byte Unknown1 { get; set; }
}
