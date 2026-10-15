using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Guild;

[Message(GameMessageOpcode.ServerGuildResult2)]
[WireMessage(GameMessageOpcode.ServerGuildResult2, Direction.ServerToClient)]
public sealed partial class ServerGuildResult2 : ServerGuildResult
{
    // Client treats opcode the same as ServerGuildResult so just use that message
    // Added here not because it is useful but to document its duplication of function
}
