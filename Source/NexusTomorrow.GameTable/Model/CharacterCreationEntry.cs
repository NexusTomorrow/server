using NexusTomorrow.Game.Static;
using NexusTomorrow.Game.Static.Entity;
using NexusTomorrow.Game.Static.Reputation;

namespace NexusTomorrow.GameTable.Model
{
    public class CharacterCreationEntry
    {
        public uint Id;
        public uint ClassId;
        public Race RaceId;
        public Sex Sex;
        public Faction FactionId;
        public bool CostumeOnly;
        [GameTableFieldArray(16u)]
        public uint[] ItemIds = [];
        public bool Enabled;
        public CharacterCreationStart CharacterCreationStartEnum;
        public uint Xp;
        public uint AccountCurrencyTypeIdCost;
        public uint AccountCurrencyAmountCost;
        public uint EntitlementIdRequired;
    }
}
