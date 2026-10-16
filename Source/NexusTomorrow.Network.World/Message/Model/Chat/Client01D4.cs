using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Chat;

[WireMessage(GameMessageOpcode.Client01D4, Direction.ClientToServer)]
public sealed partial class Client01D4
{
    [WideString] public string Unknown0 { get; private set; } = "";
    [WideString] public string Unknown1 { get; private set; } = "";

    [WideString] public string Unknown2 { get; private set; } = "";
    [Array(CountBits = 5)] public List<Client01C3Part0> Unknown3 { get; } = [];
    [UInt(16)] public ushort Unknown4 { get; set; }

    [UInt(1)] public bool Unknown5 { get; set; }
}
