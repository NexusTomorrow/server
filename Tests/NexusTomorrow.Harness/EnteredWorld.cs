using NexusTomorrow.Network.World.Message.Model.Entity;
using NexusTomorrow.Network.World.Message.Model.Movement;
using NexusTomorrow.Network.World.Message.Model.PlayerPath;
using NexusTomorrow.Network.World.Message.Model.Pregame;
using NexusTomorrow.Network.World.Message.Model.WorldEntry;

namespace NexusTomorrow.Harness;

// The eight messages the server sent to bring a character into the world, in the order they arrived.
public sealed record EnteredWorld(
    EnterWorld Enter,
    UnitCreate Unit,
    PlayerUnit Player,
    UnitPathTypeChanged UnitPath,
    WorldSettings Settings,
    MovementControl Control,
    PlayerState State,
    PathState Path);
