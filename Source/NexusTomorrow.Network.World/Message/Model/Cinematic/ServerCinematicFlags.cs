using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Cinematic;

[WireMessage(GameMessageOpcode.ServerCinematicFlags, Direction.ServerToClient)]
public sealed partial class ServerCinematicFlags
{
    [UInt(32)] public uint Delay { get; set; }
    [UInt(32)] public uint Flags { get; set; }
}
