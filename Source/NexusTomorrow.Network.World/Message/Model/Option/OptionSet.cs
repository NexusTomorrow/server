using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Option;

[WireMessage(GameMessageOpcode.Client012B, Direction.ClientToServer)]
public sealed partial class OptionSet
{
    [UInt(32)] public uint Unknown0 { get; private set; }
    [UInt(32)] public uint Unknown1 { get; private set; }
}
