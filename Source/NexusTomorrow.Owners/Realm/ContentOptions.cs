namespace NexusTomorrow.Owners.Realm;

/// <summary>
/// The <c>Content</c> configuration section.
/// </summary>
public sealed class ContentOptions
{
    /// <summary>
    /// The configuration section's name.
    /// </summary>
    public const string Section = "Content";

    /// <summary>
    /// The folder MapGenerator extracted the player's own client data into; it holds <c>tbl\</c>. Never in the repository.
    /// </summary>
    public string ClientDataPath { get; set; } = "";
}
