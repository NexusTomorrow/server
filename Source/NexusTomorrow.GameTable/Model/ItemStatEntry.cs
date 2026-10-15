using NexusTomorrow.GameTable.Static;

namespace NexusTomorrow.GameTable.Model;

public class ItemStatEntry
{
    public uint Id;
    [GameTableFieldArray(5u)]
    public ItemStatType[] ItemStatTypeEnum = [];
    [GameTableFieldArray(5u)]
    public uint[] ItemStatData = [];
}
