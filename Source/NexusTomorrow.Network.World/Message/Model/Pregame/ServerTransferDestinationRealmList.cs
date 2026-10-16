using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Pregame;

[WireMessage(GameMessageOpcode.Message0862, Direction.ServerToClient)]
public sealed partial class Message0862
{
    [WireStruct]
    public sealed partial class Message0862Part0 : Message0761Part0
    {
        [UInt(1)] public bool Unknown14 { get; set; }
    }

    [Array(CountBits = 32)] public List<Message0862Part0> Unknown0 { get; set; } = [];
}
