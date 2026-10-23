using NexusTomorrow.Settings;

namespace NexusTomorrow.Owners.Realm;

/// <summary>
/// The <c>Content</c> configuration section.
/// </summary>
[SettingsSection(Section, 7, "Owners", "Owners#knobs")]
public sealed class ContentOptions
{
    /// <summary>
    /// The configuration section's name.
    /// </summary>
    public const string Section = "Content";

    /// <summary>
    /// The folder MapGenerator extracted the player's own client data into, which holds <c>tbl\</c>; never in the repository.
    /// </summary>
    public string ClientDataPath { get; set; } = "";
}
