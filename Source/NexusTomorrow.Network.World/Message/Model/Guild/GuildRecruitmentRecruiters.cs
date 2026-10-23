using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Guild;

/// <summary>
/// Changes the recruiters listed for recruiting guilds.
/// </summary>
[WireMessage(GameMessageOpcode.Server076D, Direction.ServerToClient)]
[Verified(Rung.Purpose)]
public sealed partial class GuildRecruitmentRecruiters
{
    /// <summary>
    /// The change to one guild's recruiters.
    /// </summary>
    [WireStruct]
    public sealed partial class Change
    {
        /// <summary>
        /// The guild, by its id within the realm.
        /// </summary>
        [UInt(64)] public ulong GuildId { get; set; }

        [UInt(1)] public bool Unknown1 { get; set; }
        [UInt(32), Count] public uint Unknown2 { get; set; }

        /// <summary>
        /// The recruiters' names.
        /// </summary>
        [Array(CountFrom = nameof(Unknown2)), WideString] public List<string> Names { get; set; } = [];

        [Array(CountFrom = nameof(Unknown2)), UInt(1)] public List<bool> Unknown4 { get; set; } = [];
    }

    /// <summary>
    /// The changes, a guild each.
    /// </summary>
    [Array(CountBits = 32)] public List<Change> Changes { get; set; } = [];
}
