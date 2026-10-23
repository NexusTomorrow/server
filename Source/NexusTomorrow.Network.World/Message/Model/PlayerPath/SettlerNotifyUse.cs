using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.PlayerPath;

/// <summary>
/// Tells the player of a use of a settler improvement.
/// </summary>
/// <remarks>
/// The client formats a line of its own text with the improvement's name and raises SettlerNotifyUse.
/// </remarks>
[WireMessage(GameMessageOpcode.Server0672, Direction.ServerToClient)]
[Verified(Rung.Purpose)]
public sealed partial class SettlerNotifyUse
{
    /// <summary>
    /// The improvement, by id.
    /// </summary>
    [UInt(14)] public ushort ImprovementId { get; set; }

    [WideString] public string Unknown1 { get; set; } = "";
}
