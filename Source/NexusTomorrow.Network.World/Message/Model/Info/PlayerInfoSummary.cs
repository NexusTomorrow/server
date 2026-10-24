using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Info;

/// <summary>
/// A player's id, name and faction, as <see cref="PlayerInfoResponse"/> and <see cref="PlayerInfoDetailsResponse"/> carry them.
/// </summary>
[WireStruct]
public sealed partial class PlayerInfoSummary
{
    [UInt(3)] public byte Unknown0 { get; set; }

    /// <summary>
    /// The player, by realm-scoped id: the <see cref="PlayerInfoRequest.PlayerId"/> the answer is for.
    /// </summary>
    public RealmScopedId PlayerId { get; set; } = new();

    /// <summary>
    /// The player's name, at most 32 UTF-16 units with the terminator included: the client copies it into a buffer that size without
    /// checking its length.
    /// </summary>
    [TerminatedWideString] public string Name { get; set; } = "";

    /// <summary>
    /// The player's faction, as a Faction2 id, which add-ons read as nFactionId.
    /// </summary>
    [UInt(14)] public ushort FactionId { get; set; }
}
