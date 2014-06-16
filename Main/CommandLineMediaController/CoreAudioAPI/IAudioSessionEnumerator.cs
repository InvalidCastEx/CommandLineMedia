/*
 * Command Line Media Player - http://commandlinemedia.codeplex.com
 * Licensed using Ms-PL - http://commandlinemedia.codeplex.com/license
 */

using System;
using System.Runtime.InteropServices;

namespace CommandLineMediaController.CoreAudioAPI
{
    /// <summary>
    /// IAudioSessionEnumerator Interface
    /// </summary>
    [Guid("E2F5BB11-0570-40CA-ACDD-3AA01277DEE8"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    internal interface IAudioSessionEnumerator
    {
        /// <summary>
        /// Gets the number of session
        /// </summary>
        /// <param name="count">The number of sessions</param>
        /// <returns>HRESULT</returns>
        int GetCount(out int count);

        /// <summary>
        /// Gets the specified session
        /// </summary>
        /// <param name="index">The index of the session to get</param>
        /// <param name="session">The specified session</param>
        /// <returns>HRESULT</returns>
        int GetSession(int index, out IAudioSessionControl session);
    }
}
