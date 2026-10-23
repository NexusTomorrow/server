using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Message.Model.Chat;

/// <summary>
/// The payload of the links whose kind carries a single bit: arms 0, 2 and 3.
/// </summary>
[WireStruct]
public partial class ChatLinkFlag : IChatLinkPayload
{
    /// <summary>
    /// The client gives this value no meaning.
    /// </summary>
    [UInt(1)] public bool Unknown0 { get; set; }
}
