using NexusTomorrow.Game.Static.Option;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Option;

// Sent when server sends message 0xF1
// If options are changed during play, they are updated with other messages
[Message(GameMessageOpcode.ClientCombatOptions)]
[WireMessage(GameMessageOpcode.ClientCombatOptions, Direction.ClientToServer)]
public sealed partial class ClientCombatOptions : IReadable
{
    [UInt(4)] public CastingOptionFlags CastingOptions { get; set; }
    [UInt(1)] public bool DisableOtherPlayersLogging { get; set; }
    [UInt(14)] public CombatLogOptions CombatLogDisableFlags { get; set; }

    public void Read(GamePacketReader reader)
    {
        CastingOptions = reader.ReadEnum<CastingOptionFlags>(4u);
        DisableOtherPlayersLogging = reader.ReadBit();
        CombatLogDisableFlags = reader.ReadEnum<CombatLogOptions>(14u);
    }
}
