using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Pet;

/// <summary>
/// Sets the pet flair the player has unlocked and every pet customization the player has.
/// </summary>
[WireMessage(GameMessageOpcode.Server012E, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class PetCustomizationList
{
    /// <summary>
    /// The flair the player has unlocked, one bit per unlock index, most significant bit first: index N is bit 0x80 &gt;&gt; (N % 8) of
    /// byte N / 8. Each index set unlocks the PetFlair entry that carries it in either of its UnlockBitIndex columns.
    /// </summary>
    [FixedArray(64), UInt(8)] public byte[] UnlockedFlair { get; set; } = new byte[64];

    /// <summary>
    /// Every pet customization the player has. The client drops the customizations it held and keeps these.
    /// </summary>
    [Array(CountBits = 32)] public List<PetCustomization> Customizations { get; set; } = [];
}
