using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Info;

[WireMessage(GameMessageOpcode.ServerTimeOfDay, Direction.ServerToClient)]
public sealed partial class ServerTimeOfDay
{
    [UInt(32)] public uint TimeOfDay { get; set; } // in seconds
    [UInt(32)] public uint Season { get; set; }
    [UInt(32)] public uint LengthOfDay { get; set; } // in seconds
}
