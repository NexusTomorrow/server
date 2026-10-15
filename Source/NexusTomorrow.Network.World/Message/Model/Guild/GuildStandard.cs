using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Guild;

[WireStruct]
public sealed partial class GuildStandard : IWritable, IReadable
{
    [WireStruct]
    public sealed partial class GuildStandardPart : IWritable, IReadable
    {
        [UInt(14)] public ushort GuildStandardPartId { get; set; }
        [UInt(14)] public ushort DyeColorRampId1 { get; set; }
        [UInt(14)] public ushort DyeColorRampId2 { get; set; }
        [UInt(14)] public ushort DyeColorRampId3 { get; set; }

        public void Write(GamePacketWriter writer)
        {
            writer.Write(GuildStandardPartId, 14u);
            writer.Write(DyeColorRampId1, 14u);
            writer.Write(DyeColorRampId2, 14u);
            writer.Write(DyeColorRampId3, 14u);
        }

        public void Read(GamePacketReader reader)
        {
            GuildStandardPartId = reader.ReadUShort(14u);
            DyeColorRampId1 = reader.ReadUShort(14u);
            DyeColorRampId2 = reader.ReadUShort(14u);
            DyeColorRampId3 = reader.ReadUShort(14u);
        }
    }

    public GuildStandardPart BackgroundIcon { get; set; } = new();
    public GuildStandardPart ForegroundIcon { get; set; } = new();
    public GuildStandardPart ScanLines { get; set; } = new();

    public void Write(GamePacketWriter writer)
    {
        BackgroundIcon.Write(writer);
        ForegroundIcon.Write(writer);
        ScanLines.Write(writer);
    }

    public void Read(GamePacketReader reader)
    {
        BackgroundIcon.Read(reader);
        ForegroundIcon.Read(reader);
        ScanLines.Read(reader);
    }
}
