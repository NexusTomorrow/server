using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Cinematic;

[Message(GameMessageOpcode.ServerCinematicText)]
[WireMessage(GameMessageOpcode.ServerCinematicText, Direction.ServerToClient)]
public sealed partial class ServerCinematicText : IWritable
{
    [UInt(32)] public uint Delay { get; set; }
    [UInt(21)] public uint LocalizedTextId { get; set; }
    [UInt(1)] public bool Unused { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(Delay);
        writer.Write(LocalizedTextId, 21u);
        writer.Write(Unused);
    }
}
