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

// What a map sends a character on arrival, in the order the client takes it: PlayerUnit needs the unit its UnitCreate made,
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
        session.Send(new WorldSettings { MovementInterval = settings.MovementInterval });
        session.Send(new MovementControl { Ticket = player.ControlTicket, RebuildMovement = true, UnitId = player.UnitId });
        session.Send(new PlayerState { Unknown2 = player.Character.TotalXp, Unknown12 = PlayerStateMode, Unknown14 = PlayerStateNoValue });
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
                new Server0262Part5 { Unknown0 = PositionCommand, Unknown1 = new SetPositionCommand { Unknown0 = location.Position } },
                new Server0262Part5 { Unknown0 = FacingCommand, Unknown1 = new SetRotationCommand { Unknown0 = new Vector3(location.Facing, 0, 0) } },
            ],
            FactionId = (ushort)character.FactionId,
            BaseFactionId = (ushort)character.FactionId,
        };
    }

    // How the value is sent: 0 is an integer.
    private static Server0262Part4 Stat(byte stat, uint value) => new()
    {
        Unknown0 = stat,
        Unknown1 = 0,
        Unknown2 = new Server0262Part4.Server0262Union1Arm00 { Unknown0 = value },
    };
}
