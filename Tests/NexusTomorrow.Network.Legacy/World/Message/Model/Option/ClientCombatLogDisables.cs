using NexusTomorrow.Game.Static.Option;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Option;

// Sent whenever one of the options is changed
[Message(GameMessageOpcode.ClientCombatLogDisables)]
public class ClientCombatLogDisables : IReadable
{
    public CombatLogOptions DisableFlags { get; set; }

    public void Read(GamePacketReader reader)
    {
        DisableFlags = reader.ReadEnum<CombatLogOptions>(32u);
    }
}
