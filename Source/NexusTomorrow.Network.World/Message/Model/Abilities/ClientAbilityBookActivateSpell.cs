using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Abilities;

[WireMessage(GameMessageOpcode.Client017A, Direction.ClientToServer)]
public sealed partial class Client017A
{
    [UInt(18)] public uint Unknown0 { get; private set; }
    [UInt(1)] public bool Unknown1 { get; private set; }
}
