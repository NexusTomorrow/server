using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Abilities;

// Sent when client makes LAS change request (0xB1) and before the new LAS is sent (0x19D).
[Message(GameMessageOpcode.ServerActionSetClearCache)]
[WireMessage(GameMessageOpcode.ServerActionSetClearCache, Direction.ServerToClient)]
public sealed partial class ServerActionSetClearCache : IWritable
{
    [UInt(1)] public bool GenerateChatLogMessage { get; set; } = true; // Sniffs seem all set to true

    public void Write(GamePacketWriter writer)
    {
        writer.Write(GenerateChatLogMessage);
    }
}
