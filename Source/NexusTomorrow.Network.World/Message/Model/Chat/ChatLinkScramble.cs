using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Message.Model.Chat;

/// <summary>
/// The payload of the links whose range the client scrambles: arms 1 and 7.
/// </summary>
[WireStruct]
public partial class ChatLinkScramble : IChatLinkPayload
{
    /// <summary>
    /// The seed the client scrambles the range with.
    /// </summary>
    [UInt(32)] public uint Seed { get; set; }
}
