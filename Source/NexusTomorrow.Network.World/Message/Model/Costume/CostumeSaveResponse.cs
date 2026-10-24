using NexusTomorrow.Game.Static.Costume;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Costume;

/// <summary>
/// Reports a costume save's result: the client raises CostumeSaveResult with the type, the index and the result.
/// </summary>
[WireMessage(GameMessageOpcode.Server0257, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class CostumeSaveResponse
{
    /// <summary>
    /// The costume's index.
    /// </summary>
    [UInt(32)] public uint Index { get; set; }

    /// <summary>
    /// Whether the costume is the character's or a mannequin's.
    /// </summary>
    [UInt(2)] public CostumeType Type { get; set; }

    /// <summary>
    /// The result.
    /// </summary>
    [UInt(4)] public CostumeSaveResult Result { get; set; }
}
