using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Pvp;

[WireMessage(GameMessageOpcode.ServerDuelLeftArea, Direction.ServerToClient)]
public sealed partial class ServerDuelLeftArea
{
}
