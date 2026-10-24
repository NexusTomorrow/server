using System.Numerics;
using NexusTomorrow.Game.Static.Entity;
using NexusTomorrow.Network.World.Entity;
using NexusTomorrow.Network.World.Entity.Command;
using NexusTomorrow.Network.World.Entity.Model;
using NexusTomorrow.Network.World.Message.Model.Entity;
using NexusTomorrow.Network.World.Message.Model.Movement;
using NexusTomorrow.Network.World.Message.Model.PlayerPath;
using NexusTomorrow.Network.World.Message.Model.Pregame;
using NexusTomorrow.Network.World.Message.Model.Shared;
using NexusTomorrow.Network.World.Message.Model.WorldEntry;
using NexusTomorrow.Owners.World;
using NexusTomorrow.Persistence.Realm;

namespace NexusTomorrow.Maps;

// What a map sends a character on arrival, in the order the client takes it: PlayerUnit needs the unit its UnitCreate made, and so
// does UnitPathTypeChanged, the client's only store of a unit's path (Unit:GetPlayerPathType reads 2147483647 without it);
// WorldSettings is needed before the client sends WorldReady, and MovementControl, PlayerState and PathState are taken only once
// PlayerUnit has named the player. Every member not set goes as its default, and every member set is marked [VerifiedMember] or sits
// in a message read to Fields.
internal static class WorldEntry
{
    // The unit type whose data carries a character's name, race, class and sex.
    private const byte PlayerUnitType = 20;

    // The unit's health stat, and its level stat.
    private const byte HealthStat = 0;
    private const byte LevelStat = 10;

    // The commands that place the unit: its position, and its facing as (yaw in radians, 0, 0).
    private const byte PositionCommand = 2;
    private const byte FacingCommand = 14;

    // The client takes 3 or 4 here and any other value as 3.
    private const byte PlayerStateMode = 3;

    // The client's own value before one arrives.
    private const uint PlayerStateNoValue = 0xFFFFFFFF;

    // Our own number until stats exist.
    internal const uint PlayerHealth = 1000;

    public static void Send(Player player, MapSettings settings)
    {
        CharacterLocation location = player.Location;
        IWorldSession session = player.Session;

        session.Send(new EnterWorld { WorldId = location.WorldId, Position = location.Position });
        session.Send(PlayerCreate(player, settings.RealmId));
        session.Send(new PlayerUnit { UnitId = player.UnitId });
        session.Send(PathOf(player));
        session.Send(new WorldSettings { MovementInterval = settings.MovementInterval });
        session.Send(new MovementControl { Ticket = player.ControlTicket, RebuildMovement = true, UnitId = player.UnitId });
        session.Send(new PlayerState { Experience = player.Character.TotalXp, InputKeySet = PlayerStateMode, CostumeIndex = PlayerStateNoValue });
        session.Send(new PathState { ActivePath = player.Character.Path, UnlockedPaths = (byte)(1 << player.Character.Path) });
    }

    public static UnitCreate PlayerCreate(Player player, ushort realmId)
    {
        CharacterRecord character = player.Character;
        CharacterLocation location = player.Location;
        return new UnitCreate
        {
            UnitId = player.UnitId,
            UnitType = PlayerUnitType,
            TypeData = new Server0262Union0Arm14
            {
                // The client refuses a 0 in either id; together they are unique to the character.
                CharacterId = character.Id,
                RealmId = realmId,
                Name = character.Name,
                RaceId = character.Race,
                ClassId = character.Class,
                Gender = (Sex)character.Sex,
            },
            Stats = [Stat(HealthStat, PlayerHealth), Stat(LevelStat, character.Level)],
            Commands =
            [
                new Server0262Part5 { Unknown0 = PositionCommand, Unknown1 = new SetPositionCommand { Position = location.Position } },
                new Server0262Part5 { Unknown0 = FacingCommand, Unknown1 = new SetRotationCommand { Rotation = new Vector3(location.Facing, 0, 0) } },
            ],
            FactionId = (ushort)character.FactionId,
            BaseFactionId = (ushort)character.FactionId,
        };
    }

    // Another player's create, as a viewer takes it: the player's own create, with the sender time its first relay is fitted from,
    // and the latest velocity, commanded velocity, state and mode relayed for it, so it moves for the viewer as it does for the rest.
    public static UnitCreate RemoteCreate(Player target, ushort realmId, uint senderTime)
    {
        UnitCreate create = PlayerCreate(target, realmId);
        create.SenderTime = senderTime;
        AddIfKept(create, target.LatestVelocity);
        AddIfKept(create, target.LatestMove);
        AddIfKept(create, target.LatestState);
        AddIfKept(create, target.LatestMode);
        return create;
    }

    // The client keeps a unit's path only from this message, for any player's unit.
    public static UnitPathTypeChanged PathOf(Player player) => new() { UnitId = player.UnitId, PathType = player.Character.Path };

    private static void AddIfKept(UnitCreate create, Server0262Part5? command)
    {
        if (command is not null)
            create.Commands.Add(command);
    }

    // How the value is sent: 0 is an integer.
    private static Server0262Part4 Stat(byte stat, uint value) => new()
    {
        Unknown0 = stat,
        Unknown1 = 0,
        Unknown2 = new Server0262Part4.Server0262Union1Arm00 { Unknown0 = value },
    };
}
