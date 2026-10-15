using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Pvp;

// Fires whenever a player re-enters the dueling area while the LeftArea warning is displayed.
[WireMessage(GameMessageOpcode.ServerDuelCancelWarning, Direction.ServerToClient)]
public sealed partial class ServerDuelCancelWarning
{
}
