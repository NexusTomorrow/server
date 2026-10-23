using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Entity;

/// <summary>
/// A unit takes an item from loot.
/// </summary>
/// <remarks>
/// The client raises LootTakenBy, and flies the item from the loot to the looter.
/// </remarks>
[WireMessage(GameMessageOpcode.Server08A5, Direction.ServerToClient)]
[Verified(Rung.Purpose)]
public sealed partial class LootTaken
{
    /// <summary>
    /// The unit the loot came from, by unit id; the item flies from it when the loot is gone.
    /// </summary>
    [UInt(32)] public uint SourceUnitId { get; set; }

    /// <summary>
    /// The unit that took the item, by unit id.
    /// </summary>
    [UInt(32)] public uint LooterUnitId { get; set; }

    public Server07F2Part0 Unknown2 { get; set; } = new();
}
