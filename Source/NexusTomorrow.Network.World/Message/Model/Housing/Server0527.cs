using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Housing;

[WireMessage(GameMessageOpcode.Server0527, Direction.ServerToClient)]
public sealed partial class Server0527
{
    [WireStruct]
    public sealed partial class Server0527Part0
    {
        public Client00B5Part0 Unknown0 { get; set; }
        [WideString] public string Unknown1 { get; set; }
        [WideString] public string Unknown2 { get; set; }
    }

    [Array(CountBits = 32)] public List<Server0527Part0> Unknown0 { get; set; } = [];
}
