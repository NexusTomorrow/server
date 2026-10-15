using NexusTomorrow.Game.Static.Combat;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Combat
{
    public interface ICombatLog : IWritable
    {
        CombatLogType Type { get; }
    }
}
