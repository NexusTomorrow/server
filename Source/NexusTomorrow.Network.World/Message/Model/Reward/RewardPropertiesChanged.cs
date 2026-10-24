using NexusTomorrow.Game.Static.Reward;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Reward;

/// <summary>
/// Sets the player's reward properties. The client answers a reward property query only from these entries; for a property it never
/// received, it reads 0 for a Discrete value and 1.0 for a scalar one.
/// </summary>
[WireMessage(GameMessageOpcode.Server092C, Direction.ServerToClient)]
[Verified(Rung.Purpose)]
public sealed partial class RewardPropertiesChanged
{
    public interface IServer092CUnion0 : IWireStruct
    {
    }

    /// <summary>
    /// An AdditiveScalar value.
    /// </summary>
    [WireStruct]
    [UnionCase(0)]
    public sealed partial class Server092CUnion0Arm00 : IServer092CUnion0
    {
        /// <summary>
        /// The value, a float.
        /// </summary>
        [VerifiedMember, Float32] public float Value { get; set; }
    }

    /// <summary>
    /// A Discrete value.
    /// </summary>
    [WireStruct]
    [UnionCase(1)]
    public sealed partial class Server092CUnion0Arm01 : IServer092CUnion0
    {
        /// <summary>
        /// The value, a whole number.
        /// </summary>
        [VerifiedMember, UInt(32)] public uint Value { get; set; }
    }

    /// <summary>
    /// A MultiplicativeScalar value.
    /// </summary>
    [WireStruct]
    [UnionCase(2)]
    public sealed partial class Server092CUnion0Arm02 : IServer092CUnion0
    {
        /// <summary>
        /// The value, a float.
        /// </summary>
        [VerifiedMember, Float32] public float Value { get; set; }
    }

    public interface IServer092CUnion1 : IWireStruct
    {
    }

    [WireStruct]
    [UnionCase(0)]
    public sealed partial class Server092CUnion1Arm00 : IServer092CUnion1
    {
        [Float32] public float Unknown0 { get; set; }
    }

    [WireStruct]
    [UnionCase(1)]
    public sealed partial class Server092CUnion1Arm01 : IServer092CUnion1
    {
        [UInt(32)] public uint Unknown0 { get; set; }
    }

    [WireStruct]
    [UnionCase(2)]
    public sealed partial class Server092CUnion1Arm02 : IServer092CUnion1
    {
        [Float32] public float Unknown0 { get; set; }
    }

    /// <summary>
    /// One reward property's entry. The client finds or inserts it by <see cref="Property"/> and <see cref="Data"/>.
    /// </summary>
    [WireStruct]
    public sealed partial class Server092CPart0
    {
        [WireStruct]
        public sealed partial class Server092CPart1
        {
            [UInt(4)] public byte Unknown0 { get; set; }
            [UInt(32)] public uint Unknown1 { get; set; }
            [UInt(2)] public byte Unknown2 { get; set; }
            [Union(nameof(Unknown2))] public IServer092CUnion1 Unknown3 { get; set; } = new Server092CUnion1Arm00();
        }

        /// <summary>
        /// The reward property.
        /// </summary>
        [VerifiedMember, UInt(6)] public RewardPropertyType Property { get; set; }

        /// <summary>
        /// The property's sub-key: a currency type, a faction or an account currency for the properties that take one, else 0.
        /// </summary>
        [VerifiedMember, UInt(32)] public uint Data { get; set; }

        /// <summary>
        /// How the value counts, which selects its arm in <see cref="Value"/>: AdditiveScalar 0 and MultiplicativeScalar 2 carry a
        /// float, Discrete 1 a whole number.
        /// </summary>
        [VerifiedMember, UInt(2)] public RewardModifierValueType ValueType { get; set; }

        /// <summary>
        /// The value, on the arm <see cref="ValueType"/> selects.
        /// </summary>
        [VerifiedMember, Union(nameof(ValueType))] public IServer092CUnion0 Value { get; set; } = new Server092CUnion0Arm00();

        [Array(CountBits = 8)] public List<Server092CPart1> Unknown4 { get; set; } = [];
    }

    /// <summary>
    /// The reward properties.
    /// </summary>
    [VerifiedMember, Array(CountBits = 8)] public List<Server092CPart0> Properties { get; set; } = [];
}
