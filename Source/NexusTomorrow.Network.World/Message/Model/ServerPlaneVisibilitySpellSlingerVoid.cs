using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

[Message(GameMessageOpcode.ServerPlaneVisibilitySpellSlingerVoid)]
[WireMessage(GameMessageOpcode.ServerPlaneVisibilitySpellSlingerVoid, Direction.ServerToClient)]
public sealed partial class ServerPlaneVisibilitySpellSlingerVoid : IWritable
{
    [UInt(32)] public uint PlanesCanBeSeen { get; set; } // Not used by client
    [UInt(32)] public uint PlanesCanSee { get; set; }    // Only used for Prerequisite checks of type = 0xE8

    public void Write(GamePacketWriter writer)
    {
        writer.Write(PlanesCanBeSeen);
        writer.Write(PlanesCanSee);
    }
}
