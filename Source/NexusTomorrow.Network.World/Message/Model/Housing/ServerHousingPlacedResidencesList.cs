using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Housing;

[WireMessage(GameMessageOpcode.Server04DD, Direction.ServerToClient)]
public sealed partial class Server04DD
{
    [WireStruct]
    public sealed partial class Server04DDPart0
    {
        public Client00B5Part0 Unknown0 { get; set; }
        [WideString] public string Unknown1 { get; set; }
        [UInt(32)] public uint Unknown2 { get; set; }
    }

    [Array(CountBits = 32)] public List<Server04DDPart0> Unknown0 { get; set; } = [];
}
