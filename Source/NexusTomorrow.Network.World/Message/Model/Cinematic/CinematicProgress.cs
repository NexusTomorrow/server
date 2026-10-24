using NexusTomorrow.Game.Static.Cinematic;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Cinematic;

/// <summary>
/// Reports the cinematic's progress to the server.
/// </summary>
[WireMessage(GameMessageOpcode.Client0720, Direction.ClientToServer)]
[Verified(Rung.Purpose)]
public sealed partial class CinematicProgress
{
    /// <summary>
    /// The state the cinematic reached.
    /// </summary>
    [UInt(8)] public CinematicState State { get; private set; }

    [UInt(1)] public bool Unknown1 { get; private set; }
}
