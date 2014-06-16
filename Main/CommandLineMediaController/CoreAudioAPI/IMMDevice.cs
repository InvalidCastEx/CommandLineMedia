/*
 * Command Line Media Player - http://commandlinemedia.codeplex.com
 * Licensed using Ms-PL - http://commandlinemedia.codeplex.com/license
 */

using System;
using System.Runtime.InteropServices;

namespace CommandLineMediaController.CoreAudioAPI
{
    internal enum DataFlow
    {
        Render,
        Capture,
        All,
        EnumCount,
    }

    internal enum Role
    {
        Console,
        Multimedia,
        Communications,
        EnumCount,
    }

    /// <summary>
    /// IMMDevice Interface
    /// </summary>
    [Guid("D666063F-1587-4E43-81F1-B948E807363F"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    internal interface IMMDevice
    {
        /// <summary>
        /// Creates a COM Object with the specified interface
        /// </summary>
        /// <param name="interfaceID">Interface ID to create</param>
        /// <param name="context">The Class Context</param>
        /// <param name="activationParameters">Activation Parameters</param>
        /// <param name="instance">The new instance</param>
        /// <returns></returns>
        int Activate(ref Guid interfaceID, uint context, IntPtr activationParameters, [MarshalAs(UnmanagedType.IUnknown)] out object instance);

        // There are more items in this interface. I have only implemented what is necessary
    }
}
