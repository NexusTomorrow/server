using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Marketplace;

/// <summary>
/// The account's CREDD history; the client keeps its entries and raises CREDDOperationHistoryResults from them.
/// </summary>
[WireMessage(GameMessageOpcode.Server0978, Direction.ServerToClient)]
[Verified(Rung.Purpose)]
public sealed partial class CreddOperationHistory
{
    /// <summary>
    /// One CREDD operation in the history.
    /// </summary>
    [WireStruct]
    public sealed partial class CreddOperationHistoryEntry
    {
        /// <summary>
        /// The operation the entry records.
        /// </summary>
        [UInt(32)] public uint Operation { get; set; }

        [UInt(1)] public bool Unknown1 { get; set; }

        /// <summary>
        /// The entry's age, in minutes.
        /// </summary>
        [UInt(32)] public uint AgeMinutes { get; set; }

        public RealmScopedId Unknown3 { get; set; } = new();
        public RealmScopedId Unknown4 { get; set; } = new();

        /// <summary>
        /// The other account in the operation, by id; the client hands it to add-ons only when the account is on the player's friend
        /// list.
        /// </summary>
        [UInt(64)] public ulong OtherAccountId { get; set; }

        /// <summary>
        /// The amount, in credits.
        /// </summary>
        [UInt(64)] public ulong Amount { get; set; }
    }

    /// <summary>
    /// The history's entries.
    /// </summary>
    [Array(CountBits = 32)] public List<CreddOperationHistoryEntry> Entries { get; set; } = [];
}
