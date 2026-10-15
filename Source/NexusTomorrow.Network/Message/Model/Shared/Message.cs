using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.Message.Model.Shared;

[WireStruct]
public sealed partial class Message : IWritable
{
    [UInt(32)] public uint Index { get; set; }
    [Array(CountBits = 8), WideString] public List<string> Messages { get; set; } = [];

    public void Write(GamePacketWriter writer)
    {
        writer.Write(Index);
        writer.Write(Messages.Count, 8u);
        Messages.ForEach(writer.WriteStringWide);
    }
}
