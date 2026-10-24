using NexusTomorrow.Game.Static.Pet;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Pet;

/// <summary>
/// Reports that a pet customization failed; the client raises PetCustomizationFailed with the result.
/// </summary>
[WireMessage(GameMessageOpcode.Server068B, Direction.ServerToClient)]
[Verified(Rung.Purpose)]
public sealed partial class PetCustomizationFailed
{
    /// <summary>
    /// Why the customization failed.
    /// </summary>
    [UInt(4)] public PetCustomizeResult Result { get; set; }

    [UInt(2)] public byte Unknown1 { get; set; }
    [UInt(32)] public uint Unknown2 { get; set; }
    [UInt(16)] public ushort Unknown3 { get; set; }

    /// <summary>
    /// The flair, as an id the client looks up in its PetFlair table.
    /// </summary>
    [UInt(14)] public ushort PetFlairId { get; set; }
}
