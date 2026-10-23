using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Message.Model.Guild;

/// <summary>
/// One of a guild's active perks.
/// </summary>
[WireStruct]
public sealed partial class GuildActivePerk
{
    /// <summary>
    /// The perk, by id.
    /// </summary>
    [UInt(14)] public ushort PerkId { get; set; }

    /// <summary>
    /// How many days the perk stays active.
    /// </summary>
    [Float32] public float DaysRemaining { get; set; }
}
