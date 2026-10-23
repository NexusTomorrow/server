using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using Microsoft.Win32.SafeHandles;

namespace NexusTomorrow.Maps.Scheduling;

// A wait on a high-resolution waitable timer and the wake handles together: a timed wait on Windows steps 10 to 16 ms, the
// timer resolves to about a millisecond.
[SupportedOSPlatform("windows")]
internal sealed partial class WindowsDeadlineWait : IDeadlineWait
{
    private const uint CreateWaitableTimerHighResolution = 0x00000002;
    private const uint TimerAllAccess = 0x001F0003;

    private readonly TimerWaitHandle _timer;
    private readonly WaitHandle[] _waits;
    private readonly WaitHandle[] _wakes;

    private WindowsDeadlineWait(SafeWaitHandle timer, WaitHandle[] wakes)
    {
        _timer = new TimerWaitHandle(timer);
        _wakes = wakes;
        _waits = new WaitHandle[wakes.Length + 1];
        _waits[0] = _timer;
        Array.Copy(wakes, 0, _waits, 1, wakes.Length);
    }

    // Null when the system refuses a high-resolution timer.
    public static WindowsDeadlineWait? Create(WaitHandle[] wakes)
    {
        SafeWaitHandle timer = CreateWaitableTimerExW(IntPtr.Zero, null, CreateWaitableTimerHighResolution, TimerAllAccess);
        if (timer.IsInvalid)
        {
            timer.Dispose();
            return null;
        }

        return new WindowsDeadlineWait(timer, wakes);
    }

    public void Wait(TimeSpan timeout)
    {
        // A negative due time is relative, in 100 ns units, which are TimeSpan ticks.
        long dueTime = -Math.Max(1, timeout.Ticks);
        if (SetWaitableTimer(_timer.SafeWaitHandle, ref dueTime, 0, IntPtr.Zero, IntPtr.Zero, false))
            WaitHandle.WaitAny(_waits);
        else
            WaitHandle.WaitAny(_wakes, timeout);
    }

    public void Dispose() => _timer.Dispose();

    [LibraryImport("kernel32.dll", EntryPoint = "CreateWaitableTimerExW", SetLastError = true, StringMarshalling = StringMarshalling.Utf16)]
    private static partial SafeWaitHandle CreateWaitableTimerExW(IntPtr timerAttributes, string? timerName, uint flags, uint desiredAccess);

    [LibraryImport("kernel32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static partial bool SetWaitableTimer(SafeWaitHandle timer, ref long dueTime, int period, IntPtr completionRoutine, IntPtr argToCompletionRoutine, [MarshalAs(UnmanagedType.Bool)] bool resume);

    private sealed class TimerWaitHandle : WaitHandle
    {
        public TimerWaitHandle(SafeWaitHandle handle)
        {
            SafeWaitHandle = handle;
        }
    }
}
