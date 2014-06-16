/*
 * Command Line Media Player - http://commandlinemedia.codeplex.com
 * Licensed using Ms-PL - http://commandlinemedia.codeplex.com/license
 */

using System;
using System.Runtime.InteropServices;

namespace CommandLineMediaController.CoreAudioAPI
{
    internal enum AudioSessionState
    {
        Inactive,
        Active,
        Expired,
    }

    [Guid("BFB7FF88-7239-4FC9-8FA2-07C950BE9C6D"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    internal interface IAudioSessionControl2
    {
        /// <summary>
        /// Gets the current state
        /// </summary>
        /// <param name="state">The state</param>
        /// <returns>HRESULT</returns>
        int GetState(out AudioSessionState state);

        /// <summary>
        /// Gets the display name
        /// </summary>
        /// <param name="displayName">The display name</param>
        /// <returns>HRESULT</returns>
        int GetDisplayName([MarshalAs(UnmanagedType.LPWStr)]out string displayName);

        /// <summary>
        /// Sets the display name
        /// </summary>
        /// <param name="displayName">The display name</param>
        /// <returns>HRESULT</returns>
        int SetDisplayName([MarshalAs(UnmanagedType.LPWStr)]string displayName);

        /// <summary>
        /// Gets the icon path
        /// </summary>
        /// <param name="iconPath">The path to the icon</param>
        /// <returns>HRESULT</returns>
        int GetIconPath([MarshalAs(UnmanagedType.LPWStr)]out string iconPath);

        /// <summary>
        /// Sets the icon path
        /// </summary>
        /// <param name="iconPath">The path to the icon</param>
        /// <returns>HRESULT</returns>
        int SetIconPath([MarshalAs(UnmanagedType.LPWStr)]string iconPath);

        /// <summary>
        /// Gets the Grouping Parameter
        /// </summary>
        /// <param name="groupingParam">The grouping parameter</param>
        /// <returns>HRESULT</returns>
        int GetGroupingParam(out Guid groupingParam);

        /// <summary>
        /// Sets the grouping parameter
        /// </summary>
        /// <param name="grouping">The grouping parameter</param>
        /// <param name="eventContext">The event context</param>
        /// <returns>HRESULT</returns>
        int SetGroupingParam(Guid grouping, Guid eventContext);

        /// <summary>
        /// Registers an Audio Session Notification
        /// </summary>
        /// <param name="newNotification">The new notification to register</param>
        /// <returns>HRESULT</returns>
        /// <remarks>Intended to be used as an IAudioSessionEvents which is not being implemented to save time</remarks>
        int RegisterAudioSessionNotification(IntPtr newNotification);

        /// <summary>
        /// Unregisters an Audio Session Notification
        /// </summary>
        /// <param name="newNotification">The notification to unregister</param>
        /// <returns>HRESULT</returns>
        /// <remarks>Intended to be used as an IAudioSessionEvents which is not being implemented to save time</remarks>
        int UnregisterAudioSessionNotification(IntPtr newNotification);

        /// <summary>
        /// Gets the Session ID
        /// </summary>
        /// <param name="sessionID">The session ID</param>
        /// <returns>HRESULT</returns>
        int GetSessionIdentifier(out IntPtr sessionID);

        /// <summary>
        /// Gets the Session IID
        /// </summary>
        /// <param name="sessionInstanceID">The Session IID</param>
        /// <returns>HRESULT</returns>
        int GetSessionInstanceIdentifier(out IntPtr sessionInstanceID);

        /// <summary>
        /// Gets the process ID of the Session
        /// </summary>
        /// <param name="processID">The process ID</param>
        /// <returns>HRESULT</returns>
        int GetProcessID(out uint processID);
    }
}
