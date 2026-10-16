using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model;

[WireMessage(GameMessageOpcode.Server0935, Direction.ServerToClient)]
public sealed partial class Server0935
{
    [UInt(32)] public uint Unknown0 { get; set; }
    public Server0935Part0 Unknown1 { get; set; }
}
