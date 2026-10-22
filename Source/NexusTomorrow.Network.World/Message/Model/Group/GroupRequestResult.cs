using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Group;

/// <summary>
/// Reports how the server processed a request to join a group.
/// </summary>
/// <remarks>
/// The client raises Group_Request_Result with the three named fields.
/// </remarks>
[WireMessage(GameMessageOpcode.Server0456, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class GroupRequestResult
{
    /// <summary>
    /// The client gives this value no meaning.
    /// </summary>
    [UInt(64)] public ulong Unknown0 { get; set; }

    /// <summary>
    /// The name the event passes as strPlayerName.
    /// </summary>
    [WideString] public string PlayerName { get; set; } = "";

    /// <summary>
    /// A GroupLib.Result value, the event's eResult.
    /// </summary>
    [UInt(5)] public byte Result { get; set; }

    /// <summary>
    /// The event's bIsJoin.
    /// </summary>
    [UInt(1)] public bool IsJoin { get; set; }
}
