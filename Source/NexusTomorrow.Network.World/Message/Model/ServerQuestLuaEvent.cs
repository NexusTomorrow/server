using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

[Message(GameMessageOpcode.ServerQuestLuaEvent)]
[WireMessage(GameMessageOpcode.ServerQuestLuaEvent, Direction.ServerToClient)]
public sealed partial class ServerQuestLuaEvent : IWritable
{
    public enum LuaEventType
    {
        Int = 0,
        String = 1,
        Unit = 2,
        Bool = 3,
        Item = 4,
        Quest = 5,
    }

    public interface ILuaEventData : IWritable, IWireStruct
    {
        LuaEventType Type { get; }
    }

    [WireStruct]
    [UnionCase(LuaEventType.Int)]
    public sealed partial class LuaEventDataInt : ILuaEventData, IWritable
    {
        [NotOnWire] public LuaEventType Type => LuaEventType.Int;
        [UInt(32)] public int Int { get; set; }

        public void Write(GamePacketWriter writer)
        {
            writer.Write(Int);
        }
    }

    [WireStruct]
    [UnionCase(LuaEventType.String)]
    public sealed partial class LuaEventDataString : ILuaEventData, IWritable
    {
        [NotOnWire] public LuaEventType Type => LuaEventType.String;
        [WideString] public string Text { get; set; }

        public void Write(GamePacketWriter writer)
        {
            writer.WriteStringWide(Text);
        }
    }

    [WireStruct]
    [UnionCase(LuaEventType.Bool)]
    public sealed partial class LuaEventDataBool : ILuaEventData, IWritable
    {
        [NotOnWire] public LuaEventType Type => LuaEventType.Bool;
        [UInt(1)] public bool Bool { get; set; }

        public void Write(GamePacketWriter writer)
        {
            writer.Write(Bool);
        }
    }

    [WireStruct]
    [UnionCase(LuaEventType.Item)]
    public sealed partial class LuaEventDataItem : ILuaEventData, IWritable
    {
        [NotOnWire] public LuaEventType Type => LuaEventType.Item;
        [UInt(18)] public uint Item2Id { get; set; }

        public void Write(GamePacketWriter writer)
        {
            writer.Write(Item2Id, 18u);
        }
    }

    [WireStruct]
    [UnionCase(LuaEventType.Quest)]
    public sealed partial class LuaEventDataQuest : ILuaEventData, IWritable
    {
        [NotOnWire] public LuaEventType Type => LuaEventType.Quest;
        [UInt(15)] public uint QuestId { get; set; }

        public void Write(GamePacketWriter writer)
        {
            writer.Write(QuestId, 15u);
        }
    }

    // One element of LuaEvents: the 32-bit type, then the data it selects.
    [WireStruct]
    public sealed partial class LuaEvent
    {
        [UInt(32)] public LuaEventType Type { get; set; }
        [Union(nameof(Type))] public ILuaEventData Data { get; set; }
    }

    [UInt(16)] public ushort LuaEventId { get; set; }
    [Array(CountBits = 32)] public List<LuaEvent> LuaEvents { get; set; } = [];

    public void Write(GamePacketWriter writer)
    {
        writer.Write(LuaEventId);
        writer.Write(LuaEvents.Count);
        foreach (LuaEvent luaEvent in LuaEvents)
        {
            writer.Write(luaEvent.Data.Type, 32u);
            luaEvent.Data.Write(writer);
        }
    }
}
