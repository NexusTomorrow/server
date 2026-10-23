using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Message.Model.Shared;

/// <summary>
/// One of the unit's stats: which stat, how its value is sent, and the value.
/// </summary>
[WireStruct]
public sealed partial class Server0262Part4
{
    public interface IServer0262Union1 : IWireStruct
    {
    }

    [WireStruct]
    [UnionCase(0)]
    public sealed partial class Server0262Union1Arm00 : IServer0262Union1
    {
        /// <summary>
        /// The value as an integer.
        /// </summary>
        [VerifiedMember, UInt(32)] public uint Unknown0 { get; set; }
    }

    [WireStruct]
    [UnionCase(1)]
    public sealed partial class Server0262Union1Arm01 : IServer0262Union1
    {
        /// <summary>
        /// The value as a float. The client copies these bits into the stat's float field, so a stat the client keeps as a float (1 to 9,
        /// 19, and 23 to 25, dash energy being 9) must use this arm: any other arm leaves that stat at 0.
        /// </summary>
        [VerifiedMember, Float32] public float Unknown0 { get; set; }
    }

    [WireStruct]
    [UnionCase(2)]
    public sealed partial class Server0262Union1Arm02 : IServer0262Union1
    {
        /// <summary>
        /// The first integer of the pair. For stat 0 the client takes it as the health.
        /// </summary>
        [UInt(32)] public uint Unknown0 { get; set; }

        /// <summary>
        /// The second integer of the pair.
        /// </summary>
        [UInt(32)] public uint Unknown1 { get; set; }
    }

    /// <summary>
    /// The stat, 0 to 31: 0 is the unit's health, 10 its level, and 11 a level the client uses in place of stat 10's when it is not 0. The
    /// client ignores stats 16 to 18 and 26 to 31.
    /// </summary>
    [VerifiedMember, UInt(5)] public byte Unknown0 { get; set; }

    /// <summary>
    /// How the value is sent: 0 an integer, 1 a float, 2 a pair of integers.
    /// </summary>
    [VerifiedMember, UInt(2)] public byte Unknown1 { get; set; }

    /// <summary>
    /// The value, in the form <see cref="Unknown1"/> names.
    /// </summary>
    [VerifiedMember, Union(nameof(Unknown1))] public IServer0262Union1 Unknown2 { get; set; } = new Server0262Union1Arm00();
}
