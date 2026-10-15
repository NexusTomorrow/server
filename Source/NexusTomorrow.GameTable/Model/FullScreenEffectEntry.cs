namespace NexusTomorrow.GameTable.Model
{
    public class FullScreenEffectEntry
    {
        public uint Id;
        public string Description = string.Empty;
        public string TexturePath = string.Empty;
        public string ModelPath = string.Empty;
        public uint Priority;
        public uint FullScreenEffectTypeEnum;
        public float AlphaMinStart;
        public float AlphaMinEnd;
        public float AlphaStart;
        public float AlphaEnd;
        public float HzStart;
        public float HzEnd;
        public float SaturationStart;
        public float SaturationEnd;
    }
}
