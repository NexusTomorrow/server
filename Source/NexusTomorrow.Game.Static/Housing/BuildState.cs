namespace NexusTomorrow.Game.Static.Housing;

// Moves from Initializing -> ClearingPlot -> Starting -> InProgress -> Finishing -> Complete
public enum BuildState
{
    Initializing = 0,
    Complete = 1,
    ClearingPlot = 2,
    Starting = 3,
    InProgress = 4,
    Finishing = 5
}
