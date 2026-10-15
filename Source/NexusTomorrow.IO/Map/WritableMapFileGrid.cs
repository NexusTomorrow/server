namespace NexusTomorrow.IO.Map;

public class WritableMapFileGrid : MapFileGrid, IWritable
{
    public WritableMapFileGrid(uint x, uint y)
    {
        X = x;
        Y = y;
    }

    public void Write(BinaryWriter writer)
    {
        writer.Write(X);
        writer.Write(Y);

        writer.Write(Cells.Count);
        foreach (WritableMapFileCell cell in Cells.Values)
            cell.Write(writer);
    }

    public void AddCell(WritableMapFileCell cell)
    {
        ArgumentNullException.ThrowIfNull(cell);

        Cells.Add((cell.X << 16) | cell.Y, cell);
    }
}
