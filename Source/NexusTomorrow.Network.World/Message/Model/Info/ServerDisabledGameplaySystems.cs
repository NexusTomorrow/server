using NexusTomorrow.Game.Static.Info;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Info;

[WireMessage(GameMessageOpcode.ServerDisabledGameplaySystems, Direction.ServerToClient)]
public sealed partial class ServerDisabledGameplaySystems
{
    [UInt(8)] public GameplaySystem SystemType { get; set; }
    [UInt(32)] public uint Unused { get; set; }
    [UInt(1)] public bool Disabled { get; set; }
}
