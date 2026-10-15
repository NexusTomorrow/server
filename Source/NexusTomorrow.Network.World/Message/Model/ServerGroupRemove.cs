using NexusTomorrow.Game.Static.Group;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model;

/// <summary>
/// Tells a player they are no longer part of the group.
/// Invokes the 'Group_Leave' event in Apollo. Which is invoked only for the current player.
/// </summary>
[WireMessage(GameMessageOpcode.ServerGroupRemove, Direction.ServerToClient)]
public sealed partial class ServerGroupRemove
{
    [UInt(64)] public ulong GroupId { get; set; }
    [UInt(32)] public uint Unk0 { get; set; }
    public Identity TargetPlayer { get; set; } = new();
    [UInt(4)] public RemoveReason Reason { get; set; }
}
