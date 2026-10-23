using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Message.Model.Guild;

/// <summary>
/// A guild's messages, queue state, PvP record and creation date.
/// </summary>
[WireStruct]
public sealed partial class GuildStatus
{
    /// <summary>
    /// The guild's message of the day.
    /// </summary>
    [WideString] public string MessageOfTheDay { get; set; } = "";

    /// <summary>
    /// The guild's info message.
    /// </summary>
    [WideString] public string InfoMessage { get; set; } = "";

    /// <summary>
    /// The guild's queue state, a GuildQueueState value.
    /// </summary>
    [UInt(3)] public byte QueueState { get; set; }

    /// <summary>
    /// The guild's PvP record.
    /// </summary>
    public GuildPvpRecord Pvp { get; set; } = new();

    /// <summary>
    /// When the guild was created, in days from now.
    /// </summary>
    [Float32] public float CreationDate { get; set; }
}
