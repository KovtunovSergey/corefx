// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Runtime.InteropServices;

internal partial class Interop
{
    internal partial class CPError
    {
        public static int GetLastWin32Error()
        {
            return Marshal.GetLastWin32Error();
        }

        public static int GetHRForLastWin32Error()
        {
            int dwLastError = Marshal.GetLastWin32Error();
            if ((dwLastError & 0x80000000) == 0x80000000)
            {
                return dwLastError;
            }

            return (dwLastError & 0x0000FFFF) | unchecked((int)0x80070000);
        }
    }
}