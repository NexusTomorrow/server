namespace NexusTomorrow.Game.Static.Cinematic;

/// <summary>
/// The flags of the command that begins or ends the cinematic camera.
/// </summary>
[Flags]
public enum CameraAddFlags
{
    WhiteOut = 0x0,
    AddCamera = 0x1,

    /// <summary>
    /// Holds the camera after a fade-out, reporting it waiting, until the server continues the cinematic.
    /// </summary>
    AwaitContinue = 0x4,

    NotifyCancelCurrentCamera = 0x8
}
