using System;

namespace NexusTomorrow.ClientConnector.Native;

public struct SECURITY_ATTRIBUTES
{
    public int length;
    public IntPtr lpSecurityDescriptor;
    public bool bInheritHandle;
}
