using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Cinematic;

[WireMessage(GameMessageOpcode.ServerCinematicFullScreenEffectAdd, Direction.ServerToClient)]
public sealed partial class ServerCinematicFullScreenEffectAdd
{
    [UInt(32)] public uint Delay { get; set; }
    [UInt(32)] public uint FullScreenEffectId { get; set; }
}
