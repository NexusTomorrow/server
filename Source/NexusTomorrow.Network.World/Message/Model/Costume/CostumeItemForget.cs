using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Costume;

/// <summary>
/// Forgets an unlocked costume item.
/// </summary>
/// <remarks>
/// The client does not send it for an item it does not know, one not unlocked, or one any of the character's costumes holds.
/// </remarks>
[WireMessage(GameMessageOpcode.Client038B, Direction.ClientToServer)]
[Verified(Rung.Fields)]
public sealed partial class CostumeItemForget
{
    /// <summary>
    /// The item, by Item2 id.
    /// </summary>
    [UInt(18)] public uint ItemId { get; private set; }
}
