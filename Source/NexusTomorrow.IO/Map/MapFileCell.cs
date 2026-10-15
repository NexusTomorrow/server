using System.Numerics;
using NexusTomorrow.IO.Map.Static;

namespace NexusTomorrow.IO.Map;

public class MapFileCell : IReadable
{
    [Flags]
    public enum Flags
    {
        None = 0x00,
        Zone = 0x01,
        Height = 0x02,
        Aura = 0x04,
        Liquid = 0x08,
        ZoneBound = 0x10
    }

    public uint X { get; protected set; }
    public uint Y { get; protected set; }

    protected Flags CellFlags { get; set; }

    protected uint[]? WorldZoneIds { get; set; }
    protected byte[,]? WorldZoneBounds { get; set; }
    protected Half[,]? HeightMap { get; set; }

    public void Read(BinaryReader reader)
    {
        X = reader.ReadUInt32();
        Y = reader.ReadUInt32();
        CellFlags = (Flags)reader.ReadUInt32();

        for (int i = 0; i < 32; i++)
        {
            var flag = (Flags)(1 << i);
            if ((CellFlags & flag) == 0)
                continue;

            switch (flag)
            {
                case Flags.Zone:
                    {
                        WorldZoneIds = new uint[4];
                        for (int j = 0; j < WorldZoneIds.Length; j++)
                            WorldZoneIds[j] = reader.ReadUInt32();
                        break;
                    }
                case Flags.Height:
                    {
                        HeightMap = new Half[17, 17];
                        for (int y = 0; y < 17; y++)
                        {
                            for (int x = 0; x < 17; x++)
                                HeightMap[x, y] = reader.ReadHalf();
                        }

                        break;
                    }
                case Flags.ZoneBound:
                    {
                        WorldZoneBounds = new byte[64, 64];
                        for (int y = 0; y < 64; y++)
                        {
                            for (int x = 0; x < 64; x++)
                                WorldZoneBounds[x, y] = reader.ReadByte();
                        }

                        break;
                    }

                default:
                    throw new NotImplementedException();
            }
        }
    }

    /// <summary>
    /// Return world zone id at supplied position.
    /// </summary>
    public uint? GetWorldZoneId(Vector3 vector)
    {
        if (WorldZoneIds is null)
            return null;

        if (WorldZoneBounds is not null)
        {
            ZoneBoundFlags zoneFlags = GetZoneBoundFlags(WorldZoneBounds, vector);
            if ((zoneFlags & ZoneBoundFlags.WorldZone2) != 0)
                return WorldZoneIds[2];
            if ((zoneFlags & ZoneBoundFlags.WorldZone1) != 0)
                return WorldZoneIds[1];
            if ((zoneFlags & ZoneBoundFlags.WorldZone0) != 0)
                return WorldZoneIds[0];
        }

        return WorldZoneIds[0];
    }

    /// <summary>
    /// Return if Road March aura is active at supplied position.
    /// </summary>
    public bool HasRoadMarch(Vector3 vector)
    {
        if (WorldZoneBounds is null)
            return false;

        return (GetZoneBoundFlags(WorldZoneBounds, vector) & ZoneBoundFlags.RoadMarch) != 0;
    }

    private static ZoneBoundFlags GetZoneBoundFlags(byte[,] bounds, Vector3 vector)
    {
        float x = vector.X + MapDefines.WorldGridOrigin * MapDefines.GridSize;
        float z = vector.Z + MapDefines.WorldGridOrigin * MapDefines.GridSize;
        uint localX = ((uint)Math.Floor(x) & 31) * 2;
        uint localZ = ((uint)Math.Floor(z) & 31) * 2;
        return (ZoneBoundFlags)bounds[localX, localZ];
    }

    /// <summary>
    /// Return terrain height at supplied position.
    /// </summary>
    public float? GetTerrainHeight(Vector3 vector)
    {
        if (HeightMap is null)
            return null;

        float trueX = vector.X + MapDefines.WorldGridOrigin * MapDefines.GridSize;
        float trueZ = vector.Z + MapDefines.WorldGridOrigin * MapDefines.GridSize;

        uint vertexX = (uint)Math.Floor(trueX / 2f);
        uint localVertexX = vertexX & 15;
        uint vertexY = (uint)Math.Floor(trueZ / 2f);
        uint localVertexY = vertexY & 15;

        float p1 = (float)HeightMap[localVertexX + 1, localVertexY];
        float p2 = (float)HeightMap[localVertexX, localVertexY + 1];

        float sqX = (trueX / 2) - vertexX;
        float sqZ = (trueZ / 2) - vertexY;

        float height;
        if ((sqX + sqZ) < 1)
        {
            float p0 = (float)HeightMap[localVertexX, localVertexY];
            height = p0;
            height += (p1 - p0) * sqX;
            height += (p2 - p0) * sqZ;
        }
        else
        {
            float p3 = (float)HeightMap[localVertexX + 1, localVertexY + 1];
            height = p3;
            height += (p1 - p3) * (1.0f - sqZ);
            height += (p2 - p3) * (1.0f - sqX);
        }

        return height;
    }
}
