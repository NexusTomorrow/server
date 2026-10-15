using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

[Message(GameMessageOpcode.ServerQuestLuaEvent)]
public partial class ServerQuestLuaEvent : IWritable
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

    public interface ILuaEventData : IWritable
    {
        LuaEventType Type { get; }
    }

    [WireStruct]
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
    public sealed partial class LuaEventDataQuest : ILuaEventData, IWritable
    {
        [NotOnWire] public LuaEventType Type => LuaEventType.Quest;
        [UInt(15)] public uint QuestId { get; set; }

        public void Write(GamePacketWriter writer)
        {
            writer.Write(QuestId, 15u);
        }
    }

    public ushort LuaEventId { get; set; }
    public List<ILuaEventData> LuaEvents { get; set; } = [];

    public void Write(GamePacketWriter writer)
    {
        writer.Write(LuaEventId);
        writer.Write(LuaEvents.Count);
        foreach (ILuaEventData luaEvent in LuaEvents)
        {
            writer.Write(luaEvent.Type, 32u);
            luaEvent.Write(writer);
        }
    }
}
