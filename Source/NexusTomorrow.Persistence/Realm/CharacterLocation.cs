using System.Numerics;

namespace NexusTomorrow.Persistence.Realm;

/// <summary>
/// Where a character stands: its world, its zone, its position in world coordinates and its facing (the yaw, in radians).
/// </summary>
public readonly record struct CharacterLocation(ushort WorldId, ushort ZoneId, Vector3 Position, float Facing);
