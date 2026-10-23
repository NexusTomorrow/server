using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared.Message;

namespace NexusTomorrow.Network.World.Message.Model.Entity;

[WireMessage(GameMessageOpcode.Server0753, Direction.ServerToClient)]
public sealed partial class UnitSay
{
    public Server0738Part0 Unknown0 { get; set; } = new();
    [UInt(32)] public uint Unknown1 { get; set; }
    [UInt(1)] public bool Unknown2 { get; set; }
    [UInt(1)] public bool Unknown3 { get; set; }
    [Float32] public float Unknown4 { get; set; }
}
