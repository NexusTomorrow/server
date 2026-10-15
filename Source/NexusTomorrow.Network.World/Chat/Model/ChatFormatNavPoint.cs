using NexusTomorrow.Game.Static.Chat;
using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Chat.Model;

[WireStruct]
[UnionCase(ChatFormatType.NavPoint)]
public sealed partial class ChatFormatNavPoint : IChatFormatModel
{
    [NotOnWire] public ChatFormatType Type => ChatFormatType.NavPoint;
    [UInt(14)] public ushort MapZoneId { get; set; }
    [Float32] public float X { get; set; }
    [Float32] public float Y { get; set; }

    public void Read(GamePacketReader reader)
    {
        MapZoneId = reader.ReadUShort(14u);
        X = reader.ReadSingle();
        Y = reader.ReadSingle();
    }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(MapZoneId, 14u);
        writer.Write(X);
        writer.Write(Y);
    }
}
