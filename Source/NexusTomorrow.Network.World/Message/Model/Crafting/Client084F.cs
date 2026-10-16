using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Crafting;

[WireMessage(GameMessageOpcode.Client084F, Direction.ClientToServer)]
public sealed partial class Client084F
{
    [UInt(32)] public uint Unknown0 { get; private set; }
    [UInt(32)] public uint Unknown1 { get; private set; }
    [UInt(32)] public uint Unknown2 { get; private set; }
    public Client084FPart0 Unknown3 { get; private set; } = new Client084FPart0();
    [UInt(18)] public uint Unknown4 { get; private set; }
    [UInt(32)] public uint Unknown5 { get; private set; }
    [Array(CountBits = 3), UInt(32)] public uint[] Unknown6 { get; private set; } = [];
}
