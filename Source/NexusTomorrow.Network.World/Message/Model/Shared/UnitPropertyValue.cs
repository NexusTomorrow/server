using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Message.Model.Shared;

/// <summary>
/// One property entry: its id, base value and current value.
/// </summary>
[WireStruct]
public sealed partial class UnitPropertyValue
{
    /// <summary>
    /// The property, as an id in the client's UnitProperty2 table, 0 to 196.
    /// </summary>
    [VerifiedMember, UInt(8)] public byte Unknown0 { get; set; }

    /// <summary>
    /// The property's base value. The client hands it to add-ons as the base and folds it into the property hash it reports; for property 38
    /// the client's own rules read only the current value.
    /// </summary>
    [VerifiedMember, Float32] public float Unknown1 { get; set; }

    /// <summary>
    /// The property's current value. For property 38 it is the dash maximum, so 200 gives two dashes.
    /// </summary>
    [VerifiedMember, Float32] public float Unknown2 { get; set; }
}
