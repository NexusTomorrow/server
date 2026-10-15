using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Entity;

namespace NexusTomorrow.Network.World.Message.Model;

// If sent while on the CharacterSelect screen, loads into the Game screen
// If sent while Game screen is already active, reinitializes player data managers.
[Message(GameMessageOpcode.ServerChangeWorld)]
[WireMessage(GameMessageOpcode.ServerChangeWorld, Direction.ServerToClient)]
public sealed partial class ServerChangeWorld : IWritable
{
    [UInt(15)] public ushort WorldId { get; set; }
    public Position Position { get; set; }
    [Float32] public float Yaw { get; set; }
    [Float32] public float Pitch { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(WorldId, 15);
        Position.Write(writer);
        writer.Write(Yaw);
        writer.Write(Pitch);
    }
}
