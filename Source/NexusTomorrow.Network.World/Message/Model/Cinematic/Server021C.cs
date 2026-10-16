using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Entity;

namespace NexusTomorrow.Network.World.Message.Model.Cinematic;

[WireMessage(GameMessageOpcode.Server021C, Direction.ServerToClient)]
public sealed partial class Server021C
{
    [UInt(32)] public uint Unknown0 { get; set; }
    [UInt(32)] public uint Unknown1 { get; set; }
    [UInt(17)] public uint Unknown2 { get; set; }
    [UInt(32)] public uint Unknown3 { get; set; }
    public Client0096Part0 Unknown4 { get; set; }
    [UInt(1)] public bool Unknown5 { get; set; }
}
