using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Entity.Model;

/// <summary>
/// The data of a unit of type 22.
/// </summary>
[WireStruct]
[UnionCase(22)]
public sealed partial class Server0262Union0Arm16 : IServer0262Union0
{
    [WireStruct]
    public sealed partial class Server0262Part2
    {
        /// <summary>
        /// The client gives this value no meaning.
        /// </summary>
        [VerifiedMember, UInt(2)] public byte Unknown0 { get; set; }

        /// <summary>
        /// The client gives this value no meaning.
        /// </summary>
        [VerifiedMember] public Client0096Part0 Unknown1 { get; set; } = new();

        /// <summary>
        /// The client gives this value no meaning.
        /// </summary>
        [VerifiedMember] public Client0096Part0 Unknown2 { get; set; } = new();

        /// <summary>
        /// The client gives this value no meaning.
        /// </summary>
        [VerifiedMember, Float32] public float Unknown3 { get; set; }
    }

    /// <summary>
    /// The unit's name.
    /// </summary>
    [VerifiedMember, WideString] public string Name { get; set; } = "";

    /// <summary>
    /// The client gives this value no meaning.
    /// </summary>
    [VerifiedMember, Array(CountBits = 8)] public List<Server0262Part2> Unknown1 { get; } = [];
}
