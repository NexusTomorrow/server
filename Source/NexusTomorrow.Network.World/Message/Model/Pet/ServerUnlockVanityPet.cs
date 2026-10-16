using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Pet;

[WireMessage(GameMessageOpcode.Server01AE, Direction.ServerToClient)]
public sealed partial class Server01AE
{
    [UInt(18)] public uint Unknown0 { get; set; }
}

