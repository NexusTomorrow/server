using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Pet;

[WireMessage(GameMessageOpcode.Server0068, Direction.ServerToClient)]
public sealed partial class Server0068
{
    [UInt(32)] public uint Unknown0 { get; set; }
    [UInt(18)] public uint Unknown1 { get; set; }
    [UInt(5)] public uint Unknown2 { get; set; }
    [UInt(5)] public uint Unknown3 { get; set; }
}
