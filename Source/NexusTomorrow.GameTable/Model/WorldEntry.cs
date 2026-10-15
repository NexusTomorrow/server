using NexusTomorrow.Game.Static.Map;

namespace NexusTomorrow.GameTable.Model
{
    public class WorldEntry
    {
        public uint Id;
        public string AssetPath = string.Empty;
        public uint Flags;
        public MapType Type;
        public string ScreenPath = string.Empty;
        public string ScreenModelPath = string.Empty;
        public uint ChunkBounds00;
        public uint ChunkBounds01;
        public uint ChunkBounds02;
        public uint ChunkBounds03;
        public uint PlugAverageHeight;
        public uint LocalizedTextIdName;
        public uint MinItemLevel;
        public uint MaxItemLevel;
        public uint PrimeLevelOffset;
        public uint PrimeLevelMax;
        public uint VeteranTierScalingType;
        public uint HeroismMenaceLevel;
        public uint RewardRotationContentId;
    }
}
