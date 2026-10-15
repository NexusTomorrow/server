using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Fortune;

// Sent to the game server when the player has opened the Fortunes screen
// Sent at the same time as ClientFortunesNotifyStorefront
[WireMessage(GameMessageOpcode.ClientFortuneNotifyGame, Direction.ClientToServer)]
public sealed partial class ClientFortuneNotifyGame
{
}
