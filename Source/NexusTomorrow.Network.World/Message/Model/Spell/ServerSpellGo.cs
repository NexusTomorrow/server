using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Entity;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Spell;

[Message(GameMessageOpcode.ServerSpellGo)]
[WireMessage(GameMessageOpcode.ServerSpellGo, Direction.ServerToClient)]
public sealed partial class ServerSpellGo : IWritable
{
    [WireStruct]
    public sealed partial class MissileInfo : IWritable
    {
        public Position CasterPosition { get; set; } = new Position();
        [UInt(32)] public uint MissileTravelTime { get; set; }
        [UInt(32)] public uint TargetId { get; set; }
        public Position TargetPosition { get; set; } = new Position();
        [UInt(1)] public bool HitPosition { get; set; }

        public void Write(GamePacketWriter writer)
        {
            CasterPosition.Write(writer);
            writer.Write(MissileTravelTime);
            writer.Write(TargetId);
            TargetPosition.Write(writer);
            writer.Write(HitPosition);
        }
    }

    [UInt(32)] public uint ServerUniqueId { get; set; }
    [UInt(1)] public bool BIgnoreCooldown { get; set; }
    public Position PrimaryDestination { get; set; } = new Position();

    [Array(CountBits = 8)] public List<TargetInfo> TargetInfoData { get; set; } = [];
    [Array(CountBits = 8)] public List<InitialPosition> InitialPositionData { get; set; } = [];
    [Array(CountBits = 8)] public List<TelegraphPosition> TelegraphPositionData { get; set; } = [];
    [Array(CountBits = 8)] public List<MissileInfo> MissileInfoData { get; set; } = [];

    [SInt(8)] public sbyte Phase { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(ServerUniqueId);
        writer.Write(BIgnoreCooldown);
        PrimaryDestination.Write(writer);

        writer.Write(TargetInfoData.Count, 8u);
        TargetInfoData.ForEach(u => u.Write(writer));

        writer.Write(InitialPositionData.Count, 8u);
        InitialPositionData.ForEach(u => u.Write(writer));

        writer.Write(TelegraphPositionData.Count, 8u);
        TelegraphPositionData.ForEach(u => u.Write(writer));

        writer.Write(MissileInfoData.Count, 8u);
        MissileInfoData.ForEach(u => u.Write(writer));

        writer.Write(Phase);
    }
}
