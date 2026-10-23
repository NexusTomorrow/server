using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Spell;

/// <summary>
/// Reflects a cast.
/// </summary>
[WireMessage(GameMessageOpcode.Server07F6, Direction.ServerToClient)]
[Verified(Rung.Purpose)]
public sealed partial class SpellReflect
{
    /// <summary>
    /// The cast, by the server's id for it.
    /// </summary>
    [UInt(32)] public uint CastId { get; set; }

    [UInt(19)] public uint Unknown1 { get; set; }
    [UInt(32)] public uint Unknown2 { get; set; }
    [UInt(32)] public uint Unknown3 { get; set; }

    public Server0262Part10 Unknown4 { get; set; } = new();
}
