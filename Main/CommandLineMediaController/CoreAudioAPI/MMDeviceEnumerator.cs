/*
 * Command Line Media Player - http://commandlinemedia.codeplex.com
 * Licensed using Ms-PL - http://commandlinemedia.codeplex.com/license
 */

using System;
using System.Runtime.InteropServices;

namespace CommandLineMediaController.CoreAudioAPI
{
    // All objects imported from the Windows Core Audio API - http://msdn.microsoft.com/en-us/library/windows/desktop/dd370802(v=vs.85).aspx
    // This API is only available for Windows 7 or later.

    /// <summary>
    /// MMDeviceEnumerator instance
    /// </summary>
    [ComImport, Guid("BCDE0395-E52F-467C-8E3D-C4579291692E")]
    internal class MMDeviceEnumerator
    {
        // Actual implementation resides in the imported COM object. All interaction with this object will be done through interfaces.
    }
}
