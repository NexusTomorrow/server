using NexusTomorrow.Game.Static.Chat;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Chat;

[WireMessage(GameMessageOpcode.Server01C8, Direction.ServerToClient)]
public sealed partial class Server01C8
{
    public Server01B5Part0 Unknown0 { get; set; }
    [UInt(1)] public bool Unknown1 { get; set; }
    [UInt(1)] public bool Unknown2 { get; set; }
    [UInt(1)] public bool Unknown3 { get; set; }
    public Client00B5Part0 Unknown4 { get; set; }
    [WideString] public string Unknown5 { get; set; }
    [WideString] public string Unknown6 { get; set; }
    [UInt(3)] public byte Unknown7 { get; set; }

    [WideString] public string Unknown8 { get; set; }
    [Array(CountBits = 5)] public List<Client01C3Part1> Unknown9 { get; set; } = [];
    [UInt(1)] public bool Unknown10 { get; set; }
    [UInt(16)] public ushort Unknown11 { get; set; }

    [UInt(32)] public ulong Unknown12 { get; set; }
    [UInt(8)] public byte Unknown13 { get; set; }
}
