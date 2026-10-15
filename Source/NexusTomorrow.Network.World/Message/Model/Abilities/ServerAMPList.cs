using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Abilities;

// Originally called Eldan Augmentations. See eldanAugmentation tbl for information
[Message(GameMessageOpcode.ServerAmpList)]
[WireMessage(GameMessageOpcode.ServerAmpList, Direction.ServerToClient)]
public sealed partial class ServerAmpList : IWritable
{
    [UInt(3)] public byte SpecIndex { get; set; }
    [Array(CountBits = 7), UInt(16)] public List<ushort> Amps { get; set; } = []; // EldanAugmentationIds

    public void Write(GamePacketWriter writer)
    {
        writer.Write(SpecIndex, 3u);
        writer.Write(Amps.Count, 7u);
        foreach (ushort amp in Amps)
        {
            writer.Write(amp);
        }
    }
}
