namespace NexusTomorrow.GameTable.Model;

public class WorldLayerEntry
{
    public uint Id;
    public string Description = string.Empty;
    public float HeightScale;
    public float HeightOffset;
    public float ParallaxScale;
    public float ParallaxOffset;
    public float MetersPerTextureTile;
    public string ColorMapPath = string.Empty;
    public string NormalMapPath = string.Empty;
    public uint AverageColor;
    public uint Projection;
    public uint MaterialType;
    public uint WorldClutterId00;
    public uint WorldClutterId01;
    public uint WorldClutterId02;
    public uint WorldClutterId03;
    public float SpecularPower;
    public uint EmissiveGlow;
    public float ScrollSpeed00;
    public float ScrollSpeed01;
}
