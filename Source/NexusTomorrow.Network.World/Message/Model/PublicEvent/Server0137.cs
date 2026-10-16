using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.PublicEvent;

[WireMessage(GameMessageOpcode.Server0137, Direction.ServerToClient)]
public sealed partial class Server0137
{
    [UInt(14)] public uint Unknown0 { get; set; }
    public Server00D6Part0 Unknown1 { get; set; } = new();
}
