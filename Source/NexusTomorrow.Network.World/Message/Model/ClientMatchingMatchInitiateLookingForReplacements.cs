using NexusTomorrow.Game.Static.Matching;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

[WireMessage(GameMessageOpcode.ClientMatchingMatchInitiateLookingForReplacements, Direction.ClientToServer)]
public sealed partial class ClientMatchingMatchInitiateLookingForReplacements
{
    [UInt(32)] public Role Roles { get; private set; } // Roles being looked for
}
