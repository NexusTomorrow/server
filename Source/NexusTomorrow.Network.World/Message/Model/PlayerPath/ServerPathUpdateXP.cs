using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.PlayerPath;

[WireMessage(GameMessageOpcode.ServerPathUpdateXP, Direction.ServerToClient)]
public sealed partial class ServerPathUpdateXP
{
    [UInt(32)] public uint TotalXP { get; set; }
}
