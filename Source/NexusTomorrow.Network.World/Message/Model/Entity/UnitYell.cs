using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared.Message;

namespace NexusTomorrow.Network.World.Message.Model.Entity;

[WireMessage(GameMessageOpcode.Server075C, Direction.ServerToClient)]
public sealed partial class UnitYell
{
    public Server0738Part0 Unknown0 { get; set; } = new();
    [UInt(32)] public uint Unknown1 { get; set; }
    [UInt(18)] public uint Unknown2 { get; set; }
    [Float32] public float Unknown3 { get; set; }
}
