using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Message.Model.Shared;

/// <summary>
/// A group member's name and state, as the client copies them onto its member and GroupLib.GetInvite lists them.
/// </summary>
[WireStruct]
public sealed partial class GroupMemberInfo
{
    [WireStruct]
    public sealed partial class Server0405Part2
    {
        [UInt(16)] public ushort Unknown0 { get; set; }
        [UInt(8)] public byte Unknown1 { get; set; }
    }

    [WireStruct]
    public sealed partial class Server0405Part3
    {
        [UInt(15)] public ushort Unknown0 { get; set; }
        [UInt(16)] public ushort Unknown1 { get; set; }
    }

    /// <summary>
    /// The member's name.
    /// </summary>
    [VerifiedMember, WideString] public string Name { get; set; } = "";

    /// <summary>
    /// The member's faction, which GroupLib.GetInvite reports as nFactionId. The client compares it across the members to mark a group
    /// whose members' factions differ.
    /// </summary>
    [VerifiedMember, UInt(14)] public ushort FactionId { get; set; }

    /// <summary>
    /// The member's race, a Race table id.
    /// </summary>
    [VerifiedMember, UInt(14)] public ushort RaceId { get; set; }

    /// <summary>
    /// The member's class, a Class table id.
    /// </summary>
    [VerifiedMember, UInt(14)] public ushort ClassId { get; set; }

    [UInt(2)] public byte Unknown4 { get; set; }

    /// <summary>
    /// The member's level.
    /// </summary>
    [VerifiedMember, UInt(7)] public byte Level { get; set; }

    /// <summary>
    /// The client gives this value no meaning. Its member copy drops it, and the invite copy keeps it unread.
    /// </summary>
    [UInt(7)] public byte Unknown6 { get; set; }

    /// <summary>
    /// The member's path.
    /// </summary>
    [VerifiedMember, UInt(3)] public byte PathType { get; set; }

    [UInt(17)] public uint Unknown8 { get; set; }

    /// <summary>
    /// The client gives this value no meaning. Its member copy drops it, and the invite copy keeps it unread.
    /// </summary>
    [UInt(16)] public ushort Unknown9 { get; set; }

    /// <summary>
    /// The client gives this value no meaning. Its member copy drops it, and the invite copy keeps it unread.
    /// </summary>
    [FixedArray(5)] public Server0405Part2[] Unknown10 { get; set; } = [new(), new(), new(), new(), new()];

    /// <summary>
    /// The member this member mentors.
    /// </summary>
    [VerifiedMember] public RealmScopedId MenteeId { get; set; } = new();

    /// <summary>
    /// The client gives this value no meaning. Its member copy drops it, and the invite copy keeps it unread.
    /// </summary>
    [UInt(32)] public uint Unknown12 { get; set; }

    /// <summary>
    /// The client gives this value no meaning. Its member copy drops it, and the invite copy keeps it unread.
    /// </summary>
    [UInt(16)] public ushort Unknown13 { get; set; }

    /// <summary>
    /// The client gives this value no meaning. Its member copy drops it, and the invite copy keeps it unread.
    /// </summary>
    [UInt(16)] public ushort Unknown14 { get; set; }

    /// <summary>
    /// The client gives this value no meaning. Its member copy drops it, and the invite copy keeps it unread.
    /// </summary>
    [UInt(16)] public ushort Unknown15 { get; set; }

    /// <summary>
    /// The client gives this value no meaning. Its member copy drops it, and the invite copy keeps it unread.
    /// </summary>
    [UInt(16)] public ushort Unknown16 { get; set; }

    /// <summary>
    /// The client gives this value no meaning. Its member copy drops it, and the invite copy keeps it unread.
    /// </summary>
    [UInt(16)] public ushort Unknown17 { get; set; }

    /// <summary>
    /// The client gives this value no meaning. Its member copy drops it, and the invite copy keeps it unread.
    /// </summary>
    [UInt(16)] public ushort Unknown18 { get; set; }

    /// <summary>
    /// The client gives this value no meaning. Its member copy drops it, and the invite copy keeps it unread.
    /// </summary>
    [UInt(16)] public ushort Unknown19 { get; set; }

    /// <summary>
    /// The client gives this value no meaning. Its member copy drops it, and the invite copy keeps it unread.
    /// </summary>
    [UInt(16)] public ushort Unknown20 { get; set; }

    /// <summary>
    /// The client gives this value no meaning. Its member copy drops it, and the invite copy keeps it unread.
    /// </summary>
    [UInt(16)] public ushort Unknown21 { get; set; }

    /// <summary>
    /// The client gives this value no meaning. Its member copy drops it, and the invite copy keeps it unread.
    /// </summary>
    [UInt(16)] public ushort Unknown22 { get; set; }

    /// <summary>
    /// The client gives this value no meaning. Its member copy drops it, and the invite copy keeps it unread.
    /// </summary>
    [UInt(16)] public ushort Unknown23 { get; set; }

    /// <summary>
    /// The client gives this value no meaning. Its member copy drops it, and the invite copy keeps it unread.
    /// </summary>
    [UInt(16)] public ushort Unknown24 { get; set; }

    [UInt(14)] public ushort Unknown25 { get; set; }
    [UInt(15)] public ushort Unknown26 { get; set; }
    [UInt(32)] public uint Unknown27 { get; set; }
    [UInt(32)] public uint Unknown28 { get; set; }

    /// <summary>
    /// Whether the member is in the group's instance, as GroupLib.IsMemberInGroupInstance reports.
    /// </summary>
    [VerifiedMember, UInt(1)] public bool InGroupInstance { get; set; }

    /// <summary>
    /// The client gives this value no meaning. Its member copy drops it, and the invite copy keeps it unread.
    /// </summary>
    [UInt(32)] public uint Unknown30 { get; set; }

    /// <summary>
    /// The client gives this value no meaning. Its member copy drops it, and the invite copy keeps it unread.
    /// </summary>
    [UInt(32)] public uint Unknown31 { get; set; }

    [Array(CountBits = 32)] public List<Server0405Part3> Unknown32 { get; set; } = [];
}
