using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Utility;

/// <summary>
/// A command for the server, typed in the chat input after /c or /cc. The client sends the text as typed, with no check of its own, so
/// whatever the server does with it is the server's to gate.
/// </summary>
[WireMessage(GameMessageOpcode.Client03E0, Direction.ClientToServer)]
[Verified(Rung.Fields)]
public sealed partial class ServerCommand
{
    /// <summary>
    /// The text that follows the chat command, without the command itself.
    /// </summary>
    [VerifiedMember, WideString] public string Text { get; private set; } = "";
}
