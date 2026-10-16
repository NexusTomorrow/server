using NexusTomorrow.Game.Static.Entity.Movement.Command;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.World.Entity.Command;

namespace NexusTomorrow.Network.World.Entity;

[WireStruct]
public sealed partial class Server0262Part5
{
    [UInt(5)] public byte Unknown0 { get; set; }
    [Union(nameof(Unknown0))] public IServer0262Union2 Unknown1 { get; set; } = new Server0262Union2Arm00();
}
