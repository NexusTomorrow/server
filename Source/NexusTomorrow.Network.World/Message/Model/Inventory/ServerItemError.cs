using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Inventory;

/// <summary>
/// A generic item error; the client raises a generic error from <see cref="Error"/>.
/// </summary>
[WireMessage(GameMessageOpcode.Server056A, Direction.ServerToClient)]
[Verified(Rung.Purpose)]
public sealed partial class ServerItemError
{
    [UInt(64)] public ulong Unknown0 { get; set; }

    /// <summary>
    /// 0 when there is no error; any other value is a GameLib.CodeEnumGenericError code.
    /// </summary>
    [VerifiedMember, UInt(32)] public uint Error { get; set; }
}
