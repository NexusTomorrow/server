using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model;

[Message(GameMessageOpcode.ServerGroupMemberStatUpdate)]
[WireMessage(GameMessageOpcode.ServerGroupMemberStatUpdate, Direction.ServerToClient)]
public sealed partial class ServerGroupMemberStatUpdate : IWritable
{
    [UInt(64)] public ulong GroupId { get; set; }
    public Identity TargetPlayer { get; set; } = new();

    [UInt(7)] public byte Level { get; set; }
    [UInt(7)] public byte EffectiveLevel { get; set; }

    [UInt(17)] public uint Unk1 { get; set; }
    [UInt(16)] public ushort GroupMemberId { get; set; }

    [FixedArray(5)] public GroupCharacter.UnknownStruct0[] SomeStatList = [new(), new(), new(), new(), new()];

    [PackedFloat] public float Health { get; set; }
    [PackedFloat] public float HealthMax { get; set; }
    [PackedFloat] public float Shield { get; set; }
    [PackedFloat] public float ShieldMax { get; set; }
    [PackedFloat] public float InterruptArmor { get; set; }
    [PackedFloat] public float InterruptArmorMax { get; set; }
    [PackedFloat] public float Absorption { get; set; }
    [PackedFloat] public float AbsorptionMax { get; set; }
    [PackedFloat] public float Mana { get; set; }
    [PackedFloat] public float ManaMax { get; set; }
    [PackedFloat] public float HealingAbsorb { get; set; }
    [PackedFloat] public float HealingAbsorbMax { get; set; }

    [UInt(32)] public uint PhaseFlags1 { get; set; }
    [UInt(32)] public uint PhaseFlags2 { get; set; }
    [UInt(3)] public Game.Static.PlayerPath.Path Path { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(GroupId);
        TargetPlayer.Write(writer);

        writer.Write(Level, 7);
        writer.Write(EffectiveLevel, 7);

        writer.Write(Unk1, 17);
        writer.Write(GroupMemberId);

        for (int i = 0; i < 5; ++i)
        {
            SomeStatList[i] = new GroupCharacter.UnknownStruct0();
            SomeStatList[i].Write(writer);
        }

        writer.WritePackedFloat(Health);
        writer.WritePackedFloat(HealthMax);
        writer.WritePackedFloat(Shield);
        writer.WritePackedFloat(ShieldMax);
        writer.WritePackedFloat(InterruptArmor);
        writer.WritePackedFloat(InterruptArmorMax);
        writer.WritePackedFloat(Absorption);
        writer.WritePackedFloat(AbsorptionMax);
        writer.WritePackedFloat(Mana);
        writer.WritePackedFloat(ManaMax);
        writer.WritePackedFloat(HealingAbsorb);
        writer.WritePackedFloat(HealingAbsorbMax);

        writer.Write(PhaseFlags1);
        writer.Write(PhaseFlags2);
        writer.Write(Path, 3);
    }
}
