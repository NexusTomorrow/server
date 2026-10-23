using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Group;

/// <summary>
/// Updates a group member's level, vitals and path.
/// </summary>
/// <remarks>
/// The client stores each field on the member, for whichever of its current and previous groups has the id, where
/// GroupLib.GetGroupMember reports it. It stores a vital that GetGroupMember reports as a whole number with its fraction dropped.
/// </remarks>
[WireMessage(GameMessageOpcode.Server0466, Direction.ServerToClient)]
[Verified(Rung.Purpose)]
public sealed partial class GroupMemberStats
{
    /// <summary>
    /// The group, by id.
    /// </summary>
    [UInt(64)] public ulong GroupId { get; set; }

    /// <summary>
    /// The member.
    /// </summary>
    public RealmScopedId MemberId { get; set; } = new();

    /// <summary>
    /// The member's level, GetGroupMember's nLevel.
    /// </summary>
    [UInt(7)] public byte Level { get; set; }

    /// <summary>
    /// The member's effective level, GetGroupMember's nEffectiveLevel.
    /// </summary>
    [UInt(7)] public byte EffectiveLevel { get; set; }

    [UInt(17)] public uint Unknown4 { get; set; }
    [PackedFloat] public float Unknown5 { get; set; }

    [FixedArray(5)] public GroupMemberInfo.Server0405Part2[] Unknown6 { get; set; } = [new(), new(), new(), new(), new()];

    /// <summary>
    /// The member's health, GetGroupMember's nHealth.
    /// </summary>
    [PackedFloat] public float Health { get; set; }

    /// <summary>
    /// The member's maximum health, GetGroupMember's nHealthMax.
    /// </summary>
    [PackedFloat] public float HealthMax { get; set; }

    /// <summary>
    /// The member's shield, GetGroupMember's nShield.
    /// </summary>
    [PackedFloat] public float Shield { get; set; }

    /// <summary>
    /// The member's maximum shield, GetGroupMember's nShieldMax.
    /// </summary>
    [PackedFloat] public float ShieldMax { get; set; }

    /// <summary>
    /// The member's interrupt armor, GetGroupMember's nInterruptArmor.
    /// </summary>
    [PackedFloat] public float InterruptArmor { get; set; }

    /// <summary>
    /// The member's maximum interrupt armor, GetGroupMember's nInterruptArmorMax.
    /// </summary>
    [PackedFloat] public float InterruptArmorMax { get; set; }

    /// <summary>
    /// The member's absorption, GetGroupMember's nAbsorption.
    /// </summary>
    [PackedFloat] public float Absorption { get; set; }

    /// <summary>
    /// The member's maximum absorption, GetGroupMember's nAbsorptionMax.
    /// </summary>
    [PackedFloat] public float AbsorptionMax { get; set; }

    /// <summary>
    /// The member's mana, GetGroupMember's nMana; the client keeps the fraction.
    /// </summary>
    [PackedFloat] public float Mana { get; set; }

    /// <summary>
    /// The member's maximum mana, GetGroupMember's nManaMax; the client keeps the fraction.
    /// </summary>
    [PackedFloat] public float ManaMax { get; set; }

    /// <summary>
    /// The member's healing absorption, GetGroupMember's nHealingAbsorb.
    /// </summary>
    [PackedFloat] public float HealingAbsorb { get; set; }

    /// <summary>
    /// The member's maximum healing absorption, GetGroupMember's nHealingAbsorbMax.
    /// </summary>
    [PackedFloat] public float HealingAbsorbMax { get; set; }

    [UInt(32)] public uint Unknown19 { get; set; }
    [UInt(32)] public uint Unknown20 { get; set; }

    /// <summary>
    /// The member's path, GetGroupMember's ePathType.
    /// </summary>
    [UInt(3)] public byte PathType { get; set; }
}
