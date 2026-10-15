using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Abilities;

[WireMessage(GameMessageOpcode.ServerAbilityBook, Direction.ServerToClient)]
public sealed partial class ServerAbilities
{
    [WireStruct]
    public sealed partial class Spell
    {
        [UInt(18)] public uint Spell4BaseId { get; set; }
        [UInt(4)] public byte TierIndexAchieved { get; set; }
        [UInt(3)] public byte SpecIndex { get; set; }
    }

    [Array(CountBits = 32)] public List<Spell> Spells { get; set; } = [];
}
