using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Abilities;

// Sent when client makes LAS change request (0xB1) and before the new LAS is sent (0x19D).
[WireMessage(GameMessageOpcode.ServerActionSetClearCache, Direction.ServerToClient)]
public sealed partial class ServerActionSetClearCache
{
    [UInt(1)] public bool GenerateChatLogMessage { get; set; } = true; // Sniffs seem all set to true
}
