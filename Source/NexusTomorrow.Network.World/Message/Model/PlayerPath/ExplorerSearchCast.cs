using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.PlayerPath;

/// <summary>
/// Casts one of the explorer's search spells.
/// </summary>
/// <remarks>
/// PlayerPathLib.PathAction sends it, casting one of the three spells a GameFormula row names.
/// </remarks>
[WireMessage(GameMessageOpcode.Client0099, Direction.ClientToServer)]
[Verified(Rung.Purpose)]
public sealed partial class ExplorerSearchCast
{
    /// <summary>
    /// The cast, by the client's own id for it.
    /// </summary>
    [UInt(32)] public uint ClientCastId { get; private set; }

    [UInt(2)] public byte Unknown1 { get; private set; }

    [UInt(14)] public ushort Unknown2 { get; private set; }
}
