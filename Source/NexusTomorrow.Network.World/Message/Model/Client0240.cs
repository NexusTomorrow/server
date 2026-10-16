using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Entity;

namespace NexusTomorrow.Network.World.Message.Model;

[WireMessage(GameMessageOpcode.Client0240, Direction.ClientToServer)]
public sealed partial class Client0240
{
    [UInt(1)] public bool Unknown0 { get; private set; }
    [UInt(31)] public uint Unknown1 { get; private set; }
    [UInt(1)] public bool Unknown2 { get; private set; }
    [UInt(31)] public uint Unknown3 { get; private set; }
    public Client0096Part0 Unknown4 { get; private set; } = new Client0096Part0();
    [Float32] public float Unknown5 { get; private set; }
    [UInt(1)] public bool Unknown6 { get; private set; }
    [UInt(31)] public uint Unknown7 { get; private set; }
}
