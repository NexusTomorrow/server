using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

[Message(GameMessageOpcode.ServerTitles)]
[WireMessage(GameMessageOpcode.ServerTitles, Direction.ServerToClient)]
public sealed partial class ServerTitles : IWritable
{
    [WireStruct]
    public sealed partial class Title : IWritable
    {
        [UInt(14)] public ushort TitleId { get; set; }
        [UInt(1)] public bool Revoked { get; set; }
        [UInt(1)] public bool InSchedule { get; set; }
        [UInt(32)] public uint TimeRemaining { get; set; }

        public void Write(GamePacketWriter writer)
        {
            writer.Write(TitleId, 14u);
            writer.Write(Revoked);
            writer.Write(InSchedule);
            writer.Write(TimeRemaining);
        }
    }

    [Array(CountBits = 32)] public List<Title> Titles { get; set; } = [];

    public void Write(GamePacketWriter writer)
    {
        writer.Write(Titles.Count);
        Titles.ForEach(t => t.Write(writer));
    }
}
