using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Housing;

[Message(GameMessageOpcode.ClientHousingRemodel)]
[WireMessage(GameMessageOpcode.ClientHousingRemodel, Direction.ClientToServer)]
public sealed partial class ClientHousingRemodel : IReadable
{
    public Identity TargetResidence { get; } = new();
    [UInt(32)] public uint WallpaperId { get; private set; }
    [UInt(32)] public uint RoofDecorInfoId { get; private set; }
    [UInt(32)] public uint EntrywayDecorInfoId { get; private set; }
    [UInt(32)] public uint DoorDecorInfoId { get; private set; }
    [UInt(32)] public uint SkyWallpaperId { get; private set; }
    [UInt(32)] public uint MusicId { get; private set; }
    [UInt(32)] public uint GroundWallpaperId { get; private set; }
    [UInt(3)] public byte Operation { get; private set; } // always set to 1, can be ignored

    public void Read(GamePacketReader reader)
    {
        TargetResidence.Read(reader);
        WallpaperId = reader.ReadUInt();
        RoofDecorInfoId = reader.ReadUInt();
        EntrywayDecorInfoId = reader.ReadUInt();
        DoorDecorInfoId = reader.ReadUInt();
        SkyWallpaperId = reader.ReadUInt();
        MusicId = reader.ReadUInt();
        GroundWallpaperId = reader.ReadUInt();
        Operation = reader.ReadByte(3u);
    }
}
