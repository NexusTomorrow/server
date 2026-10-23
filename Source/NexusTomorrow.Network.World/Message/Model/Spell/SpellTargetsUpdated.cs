using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Spell;

/// <summary>
/// Updates casts' targets.
/// </summary>
[WireMessage(GameMessageOpcode.Server0810, Direction.ServerToClient)]
[Verified(Rung.Purpose)]
public sealed partial class SpellTargetsUpdated
{
    [WireStruct]
    public sealed partial class Server0810Part0
    {
        [UInt(32)] public uint Unknown0 { get; set; }
        [UInt(32)] public uint Unknown1 { get; set; }
        [UInt(32)] public uint Unknown2 { get; set; }
    };

    /// <summary>
    /// The targets updated, each with its cast.
    /// </summary>
    [Array(CountBits = 32)] public List<Server0810Part0> Targets { get; set; } = [];
}
