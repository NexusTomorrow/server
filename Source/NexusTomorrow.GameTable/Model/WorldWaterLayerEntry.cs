namespace NexusTomorrow.GameTable.Model
{
    public class WorldWaterLayerEntry
    {
        public uint Id;
        public string Description = string.Empty;
        public string RippleColorTex = string.Empty;
        public string RippleNormalTex = string.Empty;
        public float Scale;
        public float Rotation;
        public float Speed;
        public float OscFrequency;
        public float OscMagnitude;
        public float OscRotation;
        public float OscPhase;
        public float OscMinLayerWeight;
        public float OscMaxLayerWeight;
        public float OscLayerWeightPhase;
        public float MaterialBlend;
    }
}
