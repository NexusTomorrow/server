using NexusTomorrow.Network.Auth.Static;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.Auth.Message.Model;

[WireMessage(GameMessageOpcode.ServerAuthDenied, Direction.ServerToClient)]
public sealed partial class ServerAuthDenied
{
    [UInt(32)] public NpLoginResult LoginResult { get; set; }
    // if LoginResult > 27 client uses this value in some way for an error code
    [UInt(32)] public uint ErrorValue { get; set; } // Value to insert into replacement token in the localized string from the LoginResult
    [Float32] public float SuspendedDays { get; set; }
}
