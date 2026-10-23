using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Entity;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Spell;

/// <summary>
/// Confirms a cast the client started, and gives it the server's id.
/// </summary>
[WireMessage(GameMessageOpcode.Server07FD, Direction.ServerToClient)]
[Verified(Rung.Purpose)]
public sealed partial class SpellConfirmCast
{
    /// <summary>
    /// The cast, by the client's own id for it.
    /// </summary>
    [UInt(32)] public uint ClientCastId { get; set; }

    /// <summary>
    /// The server's id for the cast, which later messages find it by.
    /// </summary>
    [UInt(32)] public uint CastId { get; set; }

    /// <summary>
    /// The primary target, by unit id.
    /// </summary>
    [UInt(32)] public uint TargetUnitId { get; set; }

    public Client0096Part0 Unknown3 { get; set; } = new Client0096Part0();
    [Float32] public float Unknown4 { get; set; }

    [Array(CountBits = 8)] public List<Server0262Part12> Unknown5 { get; set; } = [];
    [Array(CountBits = 8)] public List<Server0262Part13> Unknown6 { get; set; } = [];
}
