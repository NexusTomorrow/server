using System;
using System.Runtime.CompilerServices;

namespace NexusTomorrow.Shared;

public static class EnumExtensions
{
    public static TValue As<TEnum, TValue>(this TEnum e) where TEnum : Enum
    {
        if (Unsafe.SizeOf<TEnum>() != Unsafe.SizeOf<TValue>())
            throw new ArgumentException($"{typeof(TEnum)} and {typeof(TValue)} differ in size.", nameof(e));

        return Unsafe.As<TEnum, TValue>(ref e);
    }
}
