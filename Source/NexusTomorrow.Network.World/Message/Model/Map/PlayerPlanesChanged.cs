using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Map;

[WireMessage(GameMessageOpcode.Server0172, Direction.ServerToClient)]
public sealed partial class PlayerPlanesChanged
{
    [UInt(8)] public uint Unknown0 { get; set; }
    [UInt(8)] public uint Unknown1 { get; set; }
}
