using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.PlayerPath;

[WireMessage(GameMessageOpcode.Server0667, Direction.ServerToClient)]
public sealed partial class Server0667
{
    [UInt(32)] public uint Unknown0 { get; set; }
    [FixedArray(4), UInt(32)] public uint[] Unknown1 { get; set; } = new uint[4];
}
