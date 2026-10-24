using System.Numerics;

namespace NexusTomorrow.Maps.Visibility;

// The players in the world on a grid over x and z. A cell is as wide as the drop radius, so every player within that distance of a
// viewer sits in the 3 by 3 cells around the viewer's. A cell's list is kept once made, so a player crossing back into it allocates
// nothing. Touched only on the map's tick.
internal sealed class MapGrid
{
    private readonly float _cellSize;
    private readonly Dictionary<(int X, int Z), List<Player>> _cells = [];

    public MapGrid(float cellSize)
    {
        _cellSize = cellSize;
    }

    public void Add(Player player)
    {
        player.Cell = CellOf(player.Location.Position);
        CellAt(player.Cell).Add(player);
        player.InGrid = true;
    }

    public void Remove(Player player)
    {
        if (!player.InGrid)
            return;

        CellAt(player.Cell).Remove(player);
        player.InGrid = false;
    }

    // Moves the player to the cell its kept position is in, when that changed.
    public void Update(Player player)
    {
        (int X, int Z) cell = CellOf(player.Location.Position);
        if (cell == player.Cell)
            return;

        CellAt(player.Cell).Remove(player);
        player.Cell = cell;
        CellAt(cell).Add(player);
    }

    // Adds every player in the 3 by 3 cells around the player's own, the player included.
    public void Near(Player player, List<Player> into)
    {
        for (int x = player.Cell.X - 1; x <= player.Cell.X + 1; x++)
        {
            for (int z = player.Cell.Z - 1; z <= player.Cell.Z + 1; z++)
            {
                if (_cells.TryGetValue((x, z), out List<Player>? players))
                    into.AddRange(players);
            }
        }
    }

    // Clears into, then adds every player on the grid whose kept position is within radius of center, in three dimensions and
    // inclusive. A player is filed under its cell at each flush, so it can have moved up to a flush's worth from it: one ring past the
    // radius covers that.
    public void CollectWithin(Vector3 center, float radius, List<Player> into)
    {
        into.Clear();
        (int x, int z) = CellOf(center);
        int rings = (int)MathF.Ceiling(radius / _cellSize) + 1;
        float radiusSquared = radius * radius;
        for (int cellX = x - rings; cellX <= x + rings; cellX++)
        {
            for (int cellZ = z - rings; cellZ <= z + rings; cellZ++)
            {
                if (!_cells.TryGetValue((cellX, cellZ), out List<Player>? players))
                    continue;

                foreach (Player player in players)
                {
                    if (Vector3.DistanceSquared(center, player.Location.Position) <= radiusSquared)
                        into.Add(player);
                }
            }
        }
    }

    private (int X, int Z) CellOf(Vector3 position) => ((int)MathF.Floor(position.X / _cellSize), (int)MathF.Floor(position.Z / _cellSize));

    private List<Player> CellAt((int X, int Z) cell)
    {
        if (!_cells.TryGetValue(cell, out List<Player>? players))
        {
            players = [];
            _cells.Add(cell, players);
        }

        return players;
    }
}
