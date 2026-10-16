using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Reward;

[WireMessage(GameMessageOpcode.Server08CC, Direction.ServerToClient)]
public sealed partial class Server08CC
{
    [UInt(32)] public uint Unknown0 { get; set; }
    [WideString] public string Unknown1 { get; set; } = "";
}
