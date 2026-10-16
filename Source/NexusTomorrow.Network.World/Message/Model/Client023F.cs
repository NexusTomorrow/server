using System.Diagnostics;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

[WireMessage(GameMessageOpcode.Client023F, Direction.ClientToServer)]
public sealed partial class Client023F
{
    [UInt(32)] public uint Unknown0 { get; private set; }
    [UInt(32)] public uint Unknown1 { get; private set; }
    [UInt(32)] public uint Unknown2 { get; private set; }
    [UInt(1)] public bool Unknown3 { get; private set; }
    [UInt(31)] public uint Unknown4 { get; private set; }
}
