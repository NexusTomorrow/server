using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.Message.Model.Shared;

/// <summary>
/// One realm notice, in each of the client's languages.
/// </summary>
[WireStruct]
public sealed partial class RealmNotice
{
    /// <summary>
    /// The client gives this value no meaning.
    /// </summary>
    [UInt(32)] public uint Unknown0 { get; set; }

    /// <summary>
    /// The notice's text per language: the client shows the string at its language number less one, and the first string when that one is
    /// empty. It does not check that index against the notice's own string count, so every notice carries a string for every language.
    /// </summary>
    [Array(CountBits = 8), WideString] public List<string> Translations { get; set; } = [];
}
