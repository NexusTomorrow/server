using NexusTomorrow.Game.Static.Combat;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Combat;

public interface ICombatLog : IWireStruct
{
    CombatLogType Type { get; }
}
