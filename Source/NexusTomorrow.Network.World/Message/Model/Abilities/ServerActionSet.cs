using NexusTomorrow.Game.Static.Spell;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;
using NexusTomorrow.Network.World.Message.Static;

namespace NexusTomorrow.Network.World.Message.Model.Abilities;

[WireMessage(GameMessageOpcode.ServerActionSet, Direction.ServerToClient)]
public sealed partial class ServerActionSet
{
    [WireStruct]
    public sealed partial class Action
    {
        [UInt(4)] public ShortcutType ShortcutType { get; set; }
        public ItemLocation Location { get; set; } = new();
        [UInt(32)] public uint ObjectId { get; set; }
    }

    [UInt(3)] public byte SpecIndex { get; set; }
    [UInt(2)] public byte Unlocked { get; set; } // Set to 1 to unlock spec index
    [UInt(6)] public LimitedActionSetResult Result { get; set; }
    [Array(CountBits = 6)] public List<Action> Actions { get; set; } = [];
}
