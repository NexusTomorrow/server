using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Cinematic;

[WireMessage(GameMessageOpcode.ServerCinematicWhiteOut, Direction.ServerToClient)]
public sealed partial class ServerCinematicWhiteOut
{
}
