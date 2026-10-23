using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Entity;

[WireMessage(GameMessageOpcode.Client015A, Direction.ClientToServer)]
public sealed partial class Resurrect
{
    [UInt(32)] public uint Unknown0 { get; private set; }
    [UInt(32)] public uint Unknown1 { get; private set; }
}
