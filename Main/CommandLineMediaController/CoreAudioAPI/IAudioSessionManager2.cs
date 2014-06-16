/*
 * Command Line Media Player - http://commandlinemedia.codeplex.com
 * Licensed using Ms-PL - http://commandlinemedia.codeplex.com/license
 */

using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace CommandLineMediaController.CoreAudioAPI
{
    [Guid("77AA99A0-1BD6-484F-8BC7-2C654C9A9B6F"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    internal interface IAudioSessionManager2
    {
        /// <summary>
        /// Gets the audio session control
        /// </summary>
        /// <param name="audioSessionGuid">The session Guid</param>
        /// <param name="streamFlags">Audio stream status flags</param>
        /// <param name="sessionControl">The session control</param>
        /// <returns>HRESULT</returns>
        int GetAudioSessionControl(Guid audioSessionGuid, uint streamFlags, out IntPtr sessionControl);

        /// <summary>
        /// Gets the Simple Audio Volume interface
        /// </summary>
        /// <param name="audioSessionGuid">The session Guid</param>
        /// <param name="crossProcessSession">Whether or not the request is cross process</param>
        /// <param name="audioVolume">The simple audio volume interface</param>
        /// <returns>HRESULT</returns>
        int GetSimpleAudioVolume(Guid audioSessionGuid, uint crossProcessSession, out ISimpleAudioVolume audioVolume);

        /// <summary>
        /// Gets the session enumerator
        /// </summary>
        /// <param name="sessionList">The session Enumerator</param>
        /// <returns>HRESULT</returns>
        int GetSessionEnumerator(out IAudioSessionEnumerator sessionList);
    }
}
