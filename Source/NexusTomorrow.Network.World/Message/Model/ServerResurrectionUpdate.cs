using NexusTomorrow.Game.Static.Entity;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

[WireMessage(GameMessageOpcode.ServerResurrectionUpdate, Direction.ServerToClient)]
public sealed partial class ServerResurrectionUpdate
{
    [UInt(8)] public ResurrectionType ShowRezFlags { get; set; } // 8
    [UInt(1)] public bool HasCasterRezRequest { get; set; }
}
