using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.PlayerPath;

[WireMessage(GameMessageOpcode.Server0672, Direction.ServerToClient)]
public sealed partial class Server0672
{
    [UInt(14)] public ushort Unknown0 { get; set; }
    [WideString] public string Unknown1 { get; set; }
}
