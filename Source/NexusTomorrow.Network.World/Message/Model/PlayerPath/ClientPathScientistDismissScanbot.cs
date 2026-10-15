using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.PlayerPath;

// Sent by PlayerPathLib::ScientistToggleScanbot, PlayerPathLib::PathAction, or PlayerPathLib::PathAction2 lua functions
[WireMessage(GameMessageOpcode.ClientPathScientistDismissScanbot, Direction.ClientToServer)]
public sealed partial class ClientPathScientistDismissScanbot
{
}
