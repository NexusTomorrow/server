using NexusTomorrow.Game.Static.Hazard;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Hazard;

[WireMessage(GameMessageOpcode.Server0109, Direction.ServerToClient)]
public sealed partial class Server0109
{
    [UInt(14)] public uint Unknown0 { get; set; }
    [UInt(3)] public byte Unknown1 { get; set; }
}
