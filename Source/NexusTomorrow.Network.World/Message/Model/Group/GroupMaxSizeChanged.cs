using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Group;

/// <summary>
/// Sets a group's maximum size, which GroupLib.GetGroupMaxSize reports.
/// </summary>
[WireMessage(GameMessageOpcode.Server0462, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class GroupMaxSizeChanged
{
    /// <summary>
    /// The group, by id.
    /// </summary>
    [UInt(64)] public ulong GroupId { get; set; }

    /// <summary>
    /// The client gives this value no meaning.
    /// </summary>
    [UInt(32)] public uint Unknown1 { get; set; }

    /// <summary>
    /// The maximum size.
    /// </summary>
    [UInt(32)] public uint MaxSize { get; set; }
}
