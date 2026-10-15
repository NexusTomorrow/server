using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Entity;

namespace NexusTomorrow.Network.World.Message.Model.Spell;

[Message(GameMessageOpcode.ServerSpellStart)]
[WireMessage(GameMessageOpcode.ServerSpellStart, Direction.ServerToClient)]
public sealed partial class ServerSpellStart : IWritable
{
    [WireStruct]
    public sealed partial class InitialPosition : IWritable
    {
        [UInt(32)] public uint UnitId { get; set; }
        [UInt(8)] public byte TargetFlags { get; set; }
        public Position Position { get; set; } = new Position();
        [Float32] public float Yaw { get; set; }
        [Float32] public float Pitch { get; set; }

        public void Write(GamePacketWriter writer)
        {
            writer.Write(UnitId);
            writer.Write(TargetFlags);
            Position.Write(writer);
            writer.Write(Yaw);
            writer.Write(Pitch);
        }
    }

    [WireStruct]
    public sealed partial class TelegraphPosition : IWritable
    {
        [UInt(16)] public ushort TelegraphId { get; set; }
        [UInt(32)] public uint AttachedUnitId { get; set; }
        [UInt(8)] public byte TargetFlags { get; set; }
        public Position Position { get; set; } = new Position();
        [Float32] public float Yaw { get; set; }
        [Float32] public float Pitch { get; set; }

        public void Write(GamePacketWriter writer)
        {
            writer.Write(TelegraphId);
            writer.Write(AttachedUnitId);
            writer.Write(TargetFlags);
            Position.Write(writer);
            writer.Write(Yaw);
            writer.Write(Pitch);
        }
    }

    [UInt(32)] public uint ServerUniqueId { get; set; }
    [UInt(18)] public uint Spell4Id { get; set; }
    [UInt(18)] public uint RootSpell4Id { get; set; }
    [UInt(18)] public uint ParentSpell4Id { get; set; }
    [UInt(32)] public uint CasterId { get; set; }
    [UInt(16)] public ushort Unused { get; set; } // Server did send values here but the client does not use them
    [UInt(32)] public uint PrimaryTargetId { get; set; }
    public Position FieldPosition { get; set; } = new Position();
    [Float32] public float Yaw { get; set; }

    [Array(CountBits = 8)] public List<InitialPosition> InitialPositionData { get; set; } = [];
    [Array(CountBits = 8)] public List<TelegraphPosition> TelegraphPositionData { get; set; } = [];
    [UInt(1)] public bool UserInitiatedSpellCast { get; set; }
    [UInt(1)] public bool UseCreatureOverrides { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(ServerUniqueId);
        writer.Write(Spell4Id, 18u);
        writer.Write(RootSpell4Id, 18u);
        writer.Write(ParentSpell4Id, 18u);
        writer.Write(CasterId);
        writer.Write(Unused);
        writer.Write(PrimaryTargetId);
        FieldPosition.Write(writer);
        writer.Write(Yaw);

        writer.Write(InitialPositionData.Count, 8u);
        InitialPositionData.ForEach(i => i.Write(writer));

        writer.Write(TelegraphPositionData.Count, 8u);
        TelegraphPositionData.ForEach(t => t.Write(writer));

        writer.Write(UserInitiatedSpellCast);
        writer.Write(UseCreatureOverrides);
    }
}
