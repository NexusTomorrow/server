using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Housing;

[WireMessage(GameMessageOpcode.Client050B, Direction.ClientToServer)]
public sealed partial class Client050B
{
    [UInt(3)] public byte Unknown0 { get; private set; }
    [UInt(32), Count] public uint Unknown1 { get; private set; }
    [Array(CountFrom = nameof(Unknown1))] public List<Client050BPart0> Unknown2 { get; private set; } = [];

    [Array(CountFrom = nameof(Unknown1)), UInt(1)] public List<bool> Unknown3 { get; private set; } = [];
}
