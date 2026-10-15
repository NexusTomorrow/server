namespace NexusTomorrow.IO.Map;

public class WritableMapFile : MapFile, IWritable
{
    public WritableMapFile(string asset)
    {
        Asset = asset;
    }

    public void Write(BinaryWriter writer)
    {
        writer.Write(Magic);
        writer.Write(Version);
        writer.Write(Build);
        writer.Write(Asset);

        writer.Write(Grids.Count);
        foreach (WritableMapFileGrid grid in Grids.Values)
            grid.Write(writer);
    }

    public void SetGrid(uint x, uint y, WritableMapFileGrid grid)
    {
        Grids.Add((grid.X << 16) | grid.Y, grid);
    }
}
