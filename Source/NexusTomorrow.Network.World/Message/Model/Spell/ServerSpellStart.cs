using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Entity;

namespace NexusTomorrow.Network.World.Message.Model.Spell;

[WireMessage(GameMessageOpcode.ServerSpellStart, Direction.ServerToClient)]
public sealed partial class ServerSpellStart
{
    [WireStruct]
    public sealed partial class InitialPosition
    {
        [UInt(32)] public uint UnitId { get; set; }
        [UInt(8)] public byte TargetFlags { get; set; }
        public Position Position { get; set; } = new Position();
        [Float32] public float Yaw { get; set; }
        [Float32] public float Pitch { get; set; }
    }

    [WireStruct]
    public sealed partial class TelegraphPosition
    {
        [UInt(16)] public ushort TelegraphId { get; set; }
        [UInt(32)] public uint AttachedUnitId { get; set; }
        [UInt(8)] public byte TargetFlags { get; set; }
        public Position Position { get; set; } = new Position();
        [Float32] public float Yaw { get; set; }
        [Float32] public float Pitch { get; set; }
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
}
