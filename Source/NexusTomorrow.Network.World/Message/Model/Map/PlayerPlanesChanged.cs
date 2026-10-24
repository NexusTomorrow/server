using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Map;

/// <summary>
/// Sets two values the client stores for the player; the client then re-evaluates its ClientEvent rows whose eventTypeEnum is 9.
/// </summary>
[WireMessage(GameMessageOpcode.Server0172, Direction.ServerToClient)]
[Verified(Rung.Purpose)]
public sealed partial class PlayerPlanesChanged
{
    /// <summary>
    /// The client gives this value no meaning. It stores it but never reads it.
    /// </summary>
    [UInt(8)] public uint Unknown0 { get; set; }

    [UInt(8)] public uint Unknown1 { get; set; }
}
