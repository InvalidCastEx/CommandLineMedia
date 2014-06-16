/*
 * Command Line Media Player - http://commandlinemedia.codeplex.com
 * Licensed using Ms-PL - http://commandlinemedia.codeplex.com/license
 */

using System;
using System.Runtime.InteropServices;

namespace CommandLineMediaController.CoreAudioAPI
{
    /// <summary>
    /// ISimpleAudioVolume Interface
    /// </summary>
    [Guid("87CE5498-68D6-44E5-9215-6DA47EF883D8"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    internal interface ISimpleAudioVolume
    {
        /// <summary>
        /// Sets the Master Volume Level
        /// </summary>
        /// <param name="volumeLevel">The volume level being set</param>
        /// <param name="eventContext">The event Context</param>
        /// <returns>HRESULT</returns>
        int SetMasterVolume(float volumeLevel, Guid eventContext);
        
        /// <summary>
        /// Gets the Current Master Volume Level
        /// </summary>
        /// <param name="volumeLevel">Volume Level</param>
        /// <returns>HRESULT</returns>
        int GetMasterVolume(out float volumeLevel);

        /// <summary>
        /// Sets the mute status
        /// </summary>
        /// <param name="muted">Whether or not the volume should be muted</param>
        /// <param name="eventContext">The event context</param>
        /// <returns>HRESULT</returns>
        int SetMute(bool muted, Guid eventContext);
        
        /// <summary>
        /// Gets the current Mute Status
        /// </summary>
        /// <param name="muted">Whether or not the audio is currently muted</param>
        /// <returns>HRESULT</returns>
        int GetMute(out bool muted);
    }
}
