using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Testing;

// A fresh instance of the server message an opcode names, from whichever assembly declares it.
public static class ServerMessages
{
    public static IWireMessage? Create(ushort opcode) =>
        global::NexusTomorrow.Network.WireMessages.CreateServerToClient(opcode)
        ?? global::NexusTomorrow.Network.Auth.WireMessages.CreateServerToClient(opcode)
        ?? global::NexusTomorrow.Network.World.WireMessages.CreateServerToClient(opcode);
}
