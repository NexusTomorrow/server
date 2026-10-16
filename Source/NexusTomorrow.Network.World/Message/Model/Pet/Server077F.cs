using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Pet;

[WireMessage(GameMessageOpcode.Server077F, Direction.ServerToClient)]
public sealed partial class Server077F
{
    [UInt(32)] public uint Unknown0 { get; set; }
}
