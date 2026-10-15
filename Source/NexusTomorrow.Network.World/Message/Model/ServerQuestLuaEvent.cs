using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

[WireMessage(GameMessageOpcode.ServerQuestLuaEvent, Direction.ServerToClient)]
public sealed partial class ServerQuestLuaEvent
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

    public interface ILuaEventData : IWireStruct
    {
        LuaEventType Type { get; }
    }

    [WireStruct]
    [UnionCase(LuaEventType.Int)]
    public sealed partial class LuaEventDataInt : ILuaEventData
    {
        [NotOnWire] public LuaEventType Type => LuaEventType.Int;
        [UInt(32)] public int Int { get; set; }
    }

    [WireStruct]
    [UnionCase(LuaEventType.String)]
    public sealed partial class LuaEventDataString : ILuaEventData
    {
        [NotOnWire] public LuaEventType Type => LuaEventType.String;
        [WideString] public string Text { get; set; }
    }

    [WireStruct]
    [UnionCase(LuaEventType.Bool)]
    public sealed partial class LuaEventDataBool : ILuaEventData
    {
        [NotOnWire] public LuaEventType Type => LuaEventType.Bool;
        [UInt(1)] public bool Bool { get; set; }
    }

    [WireStruct]
    [UnionCase(LuaEventType.Item)]
    public sealed partial class LuaEventDataItem : ILuaEventData
    {
        [NotOnWire] public LuaEventType Type => LuaEventType.Item;
        [UInt(18)] public uint Item2Id { get; set; }
    }

    [WireStruct]
    [UnionCase(LuaEventType.Quest)]
    public sealed partial class LuaEventDataQuest : ILuaEventData
    {
        [NotOnWire] public LuaEventType Type => LuaEventType.Quest;
        [UInt(15)] public uint QuestId { get; set; }
    }

    // One element of LuaEvents: the 32-bit type, then the data it selects.
    [WireStruct]
    public sealed partial class LuaEvent
    {
        [UInt(32)] public LuaEventType Type { get; set; }
        [Union(nameof(Type))] public ILuaEventData Data { get; set; }
    }

    [UInt(14)] public ushort LuaEventId { get; set; }
    [Array(CountBits = 32)] public List<LuaEvent> LuaEvents { get; set; } = [];
}
