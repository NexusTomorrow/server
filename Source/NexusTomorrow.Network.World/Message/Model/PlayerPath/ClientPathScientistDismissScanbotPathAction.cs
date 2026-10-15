using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.PlayerPath;

[WireMessage(GameMessageOpcode.ClientPathScientistDismissScanbotPathAction, Direction.ClientToServer)]
public sealed partial class ClientPathScientistDismissScanbotPathAction
{
}
