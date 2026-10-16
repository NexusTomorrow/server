using NexusTomorrow.Game.Static.AccountInventory;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.AccountInventory;

[WireMessage(GameMessageOpcode.Server0971, Direction.ServerToClient)]
public sealed partial class Server0971
{
    [UInt(3)] public byte Unknown0 { get; set; }
    [Float32] public float Unknown1 { get; set; }
}
