using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Story;

[WireMessage(GameMessageOpcode.Server0757, Direction.ServerToClient)]
public sealed partial class Server0757
{
    [UInt(15)] public ushort Unknown0 { get; set; }

    [UInt(1)] public bool Unknown1 { get; set; }
}
