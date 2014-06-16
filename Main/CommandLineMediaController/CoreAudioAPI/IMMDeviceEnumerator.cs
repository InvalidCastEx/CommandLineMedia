/*
 * Command Line Media Player - http://commandlinemedia.codeplex.com
 * Licensed using Ms-PL - http://commandlinemedia.codeplex.com/license
 */

using System;
using System.Runtime.InteropServices;

namespace CommandLineMediaController.CoreAudioAPI
{
    /// <summary>
    /// IMMDeviceEnumerator Interface
    /// </summary>
    [Guid("A95664D2-9614-4F35-A746-DE8DB63617E6"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    internal interface IMMDeviceEnumerator
    {
        /// <summary>
        /// Generates a collection of audio endpoint devices
        /// </summary>
        /// <param name="dataFlow">Data flow direction</param>
        /// <param name="stateMask">Device States</param>
        /// <param name="device">The list of device</param>
        /// <returns>HRESULT</returns>
        int EnumAudioEndpoints(DataFlow dataFlow, uint stateMask, out IntPtr device);

        /// <summary>
        /// Gets the default audio endpoint
        /// </summary>
        /// <param name="dataFlow">Data flow direction</param>
        /// <param name="role">Device Role</param>
        /// <param name="device">The default device</param>
        /// <returns>HRESULT</returns>
        int GetDefaultAudioEndpoint(DataFlow dataFlow, Role role, out IMMDevice device);

        // There are more items in this interface. I have only implemented what is necessary
    }
}
