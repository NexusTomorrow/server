using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Shared;

[WireStruct]
public sealed partial class Binding : IReadable, IWritable
{
    [UInt(14)] public ushort InputActionId { get; set; }
    [UInt(32)] public uint DeviceEnum00 { get; set; }
    [UInt(32)] public uint DeviceEnum01 { get; set; }
    [UInt(32)] public uint DeviceEnum02 { get; set; }
    [UInt(32)] public uint Code00 { get; set; }
    [UInt(32)] public uint Code01 { get; set; }
    [UInt(32)] public uint Code02 { get; set; }
    [UInt(32)] public uint MetaKeys00 { get; set; }
    [UInt(32)] public uint MetaKeys01 { get; set; }
    [UInt(32)] public uint MetaKeys02 { get; set; }
    [UInt(32)] public uint EventTypeEnum00 { get; set; }
    [UInt(32)] public uint EventTypeEnum01 { get; set; }
    [UInt(32)] public uint EventTypeEnum02 { get; set; }

    public void Read(GamePacketReader reader)
    {
        InputActionId = reader.ReadUShort(14u);
        DeviceEnum00 = reader.ReadUInt();
        DeviceEnum01 = reader.ReadUInt();
        DeviceEnum02 = reader.ReadUInt();
        Code00 = reader.ReadUInt();
        Code01 = reader.ReadUInt();
        Code02 = reader.ReadUInt();
        MetaKeys00 = reader.ReadUInt();
        MetaKeys01 = reader.ReadUInt();
        MetaKeys02 = reader.ReadUInt();
        EventTypeEnum00 = reader.ReadUInt();
        EventTypeEnum01 = reader.ReadUInt();
        EventTypeEnum02 = reader.ReadUInt();
    }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(InputActionId, 14u);
        writer.Write(DeviceEnum00);
        writer.Write(DeviceEnum01);
        writer.Write(DeviceEnum02);
        writer.Write(Code00);
        writer.Write(Code01);
        writer.Write(Code02);
        writer.Write(MetaKeys00);
        writer.Write(MetaKeys01);
        writer.Write(MetaKeys02);
        writer.Write(EventTypeEnum00);
        writer.Write(EventTypeEnum01);
        writer.Write(EventTypeEnum02);
    }
}
