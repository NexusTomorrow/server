using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

[WireMessage(GameMessageOpcode.ServerPlaneVisibilitySpellSlingerVoid, Direction.ServerToClient)]
public sealed partial class ServerPlaneVisibilitySpellSlingerVoid
{
    [UInt(8)] public uint PlanesCanBeSeen { get; set; } // Not used by client
    [UInt(8)] public uint PlanesCanSee { get; set; }    // Only used for Prerequisite checks of type = 0xE8
}
