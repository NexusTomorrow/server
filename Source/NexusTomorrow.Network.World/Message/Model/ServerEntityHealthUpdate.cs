using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

/// <summary>
/// This is sent to clients when other entitiy's health is updated. This forces a refresh on other's health bars, unlike ServerPlayerHealthUpdate (0x092F).
/// </summary>
[WireMessage(GameMessageOpcode.ServerEntityHealthUpdate, Direction.ServerToClient)]
public sealed partial class ServerEntityHealthUpdate
{
    [UInt(32)] public uint UnitId { get; set; }
    [UInt(32)] public uint Health { get; set; }
    [UInt(1)] public bool Unknown0 { get; set; }
}
