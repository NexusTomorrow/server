using NexusTomorrow.Game.Static.Spell;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;
using NexusTomorrow.Network.World.Message.Static;

namespace NexusTomorrow.Network.World.Message.Model.Abilities;

[Message(GameMessageOpcode.ServerActionSet)]
[WireMessage(GameMessageOpcode.ServerActionSet, Direction.ServerToClient)]
public sealed partial class ServerActionSet : IWritable
{
    [WireStruct]
    public sealed partial class Action : IWritable
    {
        [UInt(4)] public ShortcutType ShortcutType { get; set; }
        public ItemLocation Location { get; set; } = new();
        [UInt(32)] public uint ObjectId { get; set; }

        public void Write(GamePacketWriter writer)
        {
            writer.Write(ShortcutType, 4u);
            Location.Write(writer);
            writer.Write(ObjectId);
        }
    }

    [UInt(3)] public byte SpecIndex { get; set; }
    [UInt(2)] public byte Unlocked { get; set; } // Set to 1 to unlock spec index
    [UInt(6)] public LimitedActionSetResult Result { get; set; }
    [Array(CountBits = 6)] public List<Action> Actions { get; set; } = [];

    public void Write(GamePacketWriter writer)
    {
        writer.Write(SpecIndex, 3u);
        writer.Write(Unlocked, 2u);
        writer.Write(Result, 6u);
        writer.Write(Actions.Count, 6u);
        Actions.ForEach(e => e.Write(writer));
    }
}
