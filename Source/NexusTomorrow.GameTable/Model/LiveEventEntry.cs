namespace NexusTomorrow.GameTable.Model;

public class LiveEventEntry
{
    public uint Id;
    public uint LiveEventTypeEnum;
    public uint MaxValue;
    public uint Flags;
    public uint LiveEventCategoryEnum;
    public uint LiveEventIdParent;
    public uint LocalizedTextIdName;
    public uint LocalizedTextIdSummary;
    public string IconPath = string.Empty;
    public string IconPathButton = string.Empty;
    public string SpritePathTitle = string.Empty;
    public string SpritePathBackground = string.Empty;
    public uint CurrencyTypeIdEarned;
    public uint LocalizedTextIdCurrencyEarnedTooltip;
    public uint WorldLocation2IdExile;
    public uint WorldLocation2IdDominion;
}
