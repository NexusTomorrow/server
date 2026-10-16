using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Pregame;

/// <summary>
/// A realm-wide announcement, taken on every screen.
/// </summary>
[WireMessage(GameMessageOpcode.Server03E1, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class RealmAnnouncement
{
    /// <summary>
    /// The announcement's tier. On character and realm select the client passes it to its interface with the text; in the world, 0 and 1
    /// each raise an announcement of their own, and 2 and 3 do not.
    /// </summary>
    [UInt(2)] public byte Tier { get; set; }

    /// <summary>
    /// The announcement's text.
    /// </summary>
    [WideString] public string Text { get; set; } = "";
}
