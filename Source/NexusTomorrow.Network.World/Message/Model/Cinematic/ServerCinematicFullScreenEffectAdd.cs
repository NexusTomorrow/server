using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Cinematic;

[Message(GameMessageOpcode.ServerCinematicFullScreenEffectAdd)]
[WireMessage(GameMessageOpcode.ServerCinematicFullScreenEffectAdd, Direction.ServerToClient)]
public sealed partial class ServerCinematicFullScreenEffectAdd : IWritable
{
    [UInt(32)] public uint Delay { get; set; }
    [UInt(32)] public uint FullScreenEffectId { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(Delay);
        writer.Write(FullScreenEffectId);
    }
}
