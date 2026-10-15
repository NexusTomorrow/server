using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.PlayerPath;

// Sent when PlayerPathLib::PathAction or GameCommand is called, the active path is Scientist and there is no scanbot present
[WireMessage(GameMessageOpcode.ClientPathScientistRequestScanbot, Direction.ClientToServer)]
public sealed partial class ClientPathScientistRequestScanbot
{
    [UInt(32)] public uint ScanbotProfile { get; private set; }
    [UInt(1)] public bool IsGameCommand { get; private set; } // 0 = triggered by Lua function call, 1 = triggered by GameCommand
}
