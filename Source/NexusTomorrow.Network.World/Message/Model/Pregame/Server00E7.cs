using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Pregame;

/// <summary>
/// Names one character of the list on character select.
/// </summary>
/// <remarks>
/// Taken on character select, which ignores a character the list does not hold. In the world the client receives it and
/// ignores it.
/// </remarks>
[WireMessage(GameMessageOpcode.Server00E7, Direction.ServerToClient)]
[Verified(Rung.Purpose)]
public sealed partial class Server00E7
{
    /// <summary>
    /// The character, as listed in the character list (0x0117).
    /// </summary>
    [UInt(64)] public ulong CharacterId { get; set; }

    [UInt(1)] public bool Unknown1 { get; set; }
}
