using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Spell;

/// <summary>
/// Ends a cast with a result for each target.
/// </summary>
/// <remarks>
/// The client marks the cast removed.
/// </remarks>
[WireMessage(GameMessageOpcode.Server07FB, Direction.ServerToClient)]
[Verified(Rung.Purpose)]
public sealed partial class SpellTargetResults
{
    [WireStruct]
    public sealed partial class Server07FBPart0
    {
        [UInt(32)] public uint Unknown0 { get; set; }
        [UInt(4)] public byte Unknown1 { get; set; }
        [UInt(32)] public uint Unknown2 { get; set; }
    }

    /// <summary>
    /// The cast, by the server's id for it.
    /// </summary>
    [UInt(32)] public uint CastId { get; set; }

    [Array(CountBits = 32)] public List<Server07FBPart0> Unknown1 { get; set; } = [];
}
