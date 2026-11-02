using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Loot;

/// <summary>
/// A lootable object the client can now loot; the client marks it as available.
/// </summary>
[WireMessage(GameMessageOpcode.Server089F, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class ServerLootCanLoot
{
    /// <summary>
    /// The object's id, matched against the client's lootable list so it can be looted.
    /// </summary>
    [VerifiedMember, UInt(32)] public uint LootId { get; set; }
}
