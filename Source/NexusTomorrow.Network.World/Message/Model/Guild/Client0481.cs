using NexusTomorrow.Game.Static.Guild;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Guild;

[WireMessage(GameMessageOpcode.Client0481, Direction.ClientToServer)]
public sealed partial class Client0481
{
    [UInt(32)] public uint Unknown0 { get; private set; }
    [UInt(4)] public byte Unknown1 { get; private set; }
    [WideString] public string Unknown2 { get; private set; }
    [WideString] public string Unknown3 { get; private set; }
    [WideString] public string Unknown4 { get; private set; }
    [WideString] public string Unknown5 { get; private set; }
    public Client0481Part0 Unknown6 { get; private set; } = new();
    [UInt(1)] public bool Unknown7 { get; private set; }
}
