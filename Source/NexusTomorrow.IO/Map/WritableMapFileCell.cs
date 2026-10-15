using NexusTomorrow.IO.Area;

namespace NexusTomorrow.IO.Map;

public class WritableMapFileCell : MapFileCell, IWritable
{
    public WritableMapFileCell(ChnkCell cell)
    {
        X = cell.X;
        Y = cell.Y;

        if ((cell.Flags & ChnkCellFlags.Zone) != 0)
        {
            worldZoneIds = new uint[4];
            Array.Copy(cell.WorldZoneIds, worldZoneIds, 4);
            flags |= Flags.Zone;
        }

        if ((cell.Flags & ChnkCellFlags.HeightMap) != 0)
        {
            heightMap = new Half[17, 17];
            for (int y = 0; y < 17; y++)
            {
                for (int x = 0; x < 17; x++)
                    heightMap[x, y] = (Half)((cell.Heightmap[x + 1, y + 1] / 8.0f) - 2048f);
            }

            flags |= Flags.Height;
        }

        if ((cell.Flags & ChnkCellFlags.ZoneBound) != 0)
        {
            worldZoneBounds = new byte[64, 64];
            for (int y = 0; y < 64; y++)
            {
                for (int x = 0; x < 64; x++)
                    worldZoneBounds[x, y] = cell.WorldZoneBounds[x, y];
            }

            flags |= Flags.ZoneBound;
        }
    }

    public void Write(BinaryWriter writer)
    {
        writer.Write(X);
        writer.Write(Y);
        writer.Write((uint)flags);

        for (int i = 0; i < 32; i++)
        {
            var flag = (Flags)(1 << i);
            if ((flags & flag) == 0)
                continue;

            switch (flag)
            {
                case Flags.Zone:
                    {
                        uint[] zoneIds = worldZoneIds ?? throw new InvalidOperationException($"{nameof(Flags.Zone)} is set without {nameof(worldZoneIds)}.");
                        foreach (uint worldZoneId in zoneIds)
                            writer.Write(worldZoneId);
                        break;
                    }
                case Flags.Height:
                    {
                        Half[,] heights = heightMap ?? throw new InvalidOperationException($"{nameof(Flags.Height)} is set without {nameof(heightMap)}.");
                        for (int y = 0; y < 17; y++)
                        {
                            for (int x = 0; x < 17; x++)
                                writer.Write(heights[x, y]);
                        }

                        break;
                    }
                case Flags.ZoneBound:
                    {
                        byte[,] bounds = worldZoneBounds ?? throw new InvalidOperationException($"{nameof(Flags.ZoneBound)} is set without {nameof(worldZoneBounds)}.");
                        for (int y = 0; y < 64; y++)
                        {
                            for (int x = 0; x < 64; x++)
                                writer.Write(bounds[x, y]);
                        }

                        break;
                    }
            }
        }
    }
}
