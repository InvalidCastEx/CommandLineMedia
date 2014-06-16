/*
 * Command Line Media Player - http://commandlinemedia.codeplex.com
 * Licensed using Ms-PL - http://commandlinemedia.codeplex.com/license
 */

using CommandLineMediaController.CoreAudioAPI;
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace CommandLineMediaController
{
    public class Program
    {
        /// <summary>
        /// Sends a Message to the specified window
        /// </summary>
        /// <param name="hwnd">The Window Handle of the process to send the message to.</param>
        /// <param name="message">The message to send.</param>
        /// <param name="wParam">Unused Parameter for this application (set to 0)</param>
        /// <param name="lParam">Operation Parameter</param>
        /// <returns>Send Message Result Code</returns>
        [DllImport("user32.dll")]
        private static extern int SendMessage(IntPtr hwnd, uint message, int wParam, int lParam);

        /// <summary>
        /// Attaches to the parent console if it exists
        /// </summary>
        /// <param name="processID">The process id (-1 means parent)</param>
        /// <returns></returns>
        [DllImport("kernel32.dll")]
        private static extern int AttachConsole(int processID);

        // Constants for sending the commands to the appliction
        // See: http://msdn.microsoft.com/en-us/library/windows/desktop/ms646275(v=vs.85).aspx
        private const uint WM_APPCOMMAND = 0x0319;

        // The value of the key from the link above must be used in the upper 2 bytes of the parameter
        private const int APPCOMMAND_MEDIA_PLAY_PAUSE = 0x000E0000;
        private const int APPCOMMAND_MEDIA_PLAY = 0x002E0000;
        private const int APPCOMMAND_MEDIA_PAUSE = 0x002F0000;
        private const int APPCOMMAND_MEDIA_STOP = 0x000D0000;
        private const int APPCOMMAND_VOLUME_MUTE = 0x00080000;
        private const int APPCOMMAND_VOLUME_UP = 0x000A0000;
        private const int APPCOMMAND_VOLUME_DOWN = 0x00090000;
        private const int APPCOMMAND_MEDIA_NEXTTRACK = 0x000B0000;
        private const int APPCOMMAND_MEDIA_PREVIOUSTRACK = 0x000C0000;

        private const float VolumeIncrement = 0.1f;
        private const float MaxVolume = 1.0f;
        private const float MinVolume = 0.0f;

        /// <summary>
        /// Application entry point
        /// </summary>
        /// <param name="args">Command line arguments</param>
        public static void Main(string[] args)
        {
            try
            {
                // If we were called from a command line then attache to the parent console.
                // We are running as a Windows Application instead of a Console application but
                // we still want to be able to write to the console.
                AttachConsole(-1);

                if (args != null && args.Length >= 1 && args[0] != null && !args[0].Equals("-?"))
                {
                    // First command line argument must contain the process name or -?
                    Process selectedProcess = null;
                    ISimpleAudioVolume volumeControl = null;

                    FindMatchingProcess(args[0], out selectedProcess, out volumeControl);

                    if (selectedProcess != null)
                    {
                        Console.WriteLine("Selected Process: " + selectedProcess.ProcessName);

                        // Loop through the rest of the parameters and send the message
                        for (int argIndex = 1; argIndex < args.Length; argIndex++)
                        {
                            switch (args[argIndex])
                            {
                                case "-pp":
                                {
                                    SendMessage(selectedProcess.MainWindowHandle, WM_APPCOMMAND, 0, APPCOMMAND_MEDIA_PLAY_PAUSE);
                                    Console.WriteLine("Message Sent: Play/Pause");
                                    break;
                                }
                                case "-p":
                                {
                                    SendMessage(selectedProcess.MainWindowHandle, WM_APPCOMMAND, 0, APPCOMMAND_MEDIA_PLAY);
                                    Console.WriteLine("Message Sent: Play");
                                    break;
                                }
                                case "-pa":
                                {
                                    SendMessage(selectedProcess.MainWindowHandle, WM_APPCOMMAND, 0, APPCOMMAND_MEDIA_PAUSE);
                                    Console.WriteLine("Message Sent: Pause");
                                    break;
                                }
                                case "-s":
                                {
                                    SendMessage(selectedProcess.MainWindowHandle, WM_APPCOMMAND, 0, APPCOMMAND_MEDIA_STOP);
                                    Console.WriteLine("Message Sent: Stop");
                                    break;
                                }
                                case "-vm":
                                {
                                    if (volumeControl != null)
                                    {
                                        // Send using the Core Audio API
                                        bool muted;

                                        // Get the current state
                                        volumeControl.GetMute(out muted);

                                        // Toggle the state
                                        volumeControl.SetMute(!muted, Guid.Empty);
                                    }
                                    else
                                    {
                                        SendMessage(selectedProcess.MainWindowHandle, WM_APPCOMMAND, 0, APPCOMMAND_VOLUME_MUTE);
                                    }

                                    Console.WriteLine("Message Sent: Volume Mute");
                                    break;
                                }
                                case "-vu":
                                {
                                    if (volumeControl != null)
                                    {
                                        float currentVolume;

                                        // Get the Current Volume Level
                                        volumeControl.GetMasterVolume(out currentVolume);

                                        // Increment the volume
                                        if (currentVolume + VolumeIncrement >= MaxVolume)
                                        {
                                            volumeControl.SetMasterVolume(MaxVolume, Guid.Empty);
                                        }
                                        else
                                        {
                                            volumeControl.SetMasterVolume(currentVolume + VolumeIncrement, Guid.Empty);
                                        }
                                    }
                                    else
                                    {
                                        SendMessage(selectedProcess.MainWindowHandle, WM_APPCOMMAND, 0, APPCOMMAND_VOLUME_UP);
                                    }

                                    Console.WriteLine("Message Sent: Volume Up");
                                    break;
                                }
                                case "-vd":
                                {
                                    if (volumeControl != null)
                                    {
                                        float currentVolume;

                                        // Get the current volume level
                                        volumeControl.GetMasterVolume(out currentVolume);

                                        // Decrement the volume
                                        if (currentVolume - VolumeIncrement <= MinVolume)
                                        {
                                            volumeControl.SetMasterVolume(MinVolume, Guid.Empty);
                                        }
                                        else
                                        {
                                            volumeControl.SetMasterVolume(currentVolume - VolumeIncrement, Guid.Empty);
                                        }
                                    }
                                    else
                                    {
                                        SendMessage(selectedProcess.MainWindowHandle, WM_APPCOMMAND, 0, APPCOMMAND_VOLUME_DOWN);
                                    }

                                    Console.WriteLine("Message Sent: Volume Down");
                                    break;
                                }
                                case "-mvm":
                                {
                                    SendMessage(selectedProcess.MainWindowHandle, WM_APPCOMMAND, 0, APPCOMMAND_VOLUME_MUTE);
                                    Console.WriteLine("Message Sent: Master Volume Mute");
                                    break;
                                }
                                case "-mvu":
                                {
                                    SendMessage(selectedProcess.MainWindowHandle, WM_APPCOMMAND, 0, APPCOMMAND_VOLUME_UP);
                                    Console.WriteLine("Message Sent: Master Volume Up");
                                    break;
                                }
                                case "-mvd":
                                {
                                    SendMessage(selectedProcess.MainWindowHandle, WM_APPCOMMAND, 0, APPCOMMAND_VOLUME_DOWN);
                                    Console.WriteLine("Message Sent: Master Volume Down");
                                    break;
                                }
                                case "-nt":
                                {
                                    SendMessage(selectedProcess.MainWindowHandle, WM_APPCOMMAND, 0, APPCOMMAND_MEDIA_NEXTTRACK);
                                    Console.WriteLine("Message Sent: Next Track");
                                    break;
                                }
                                case "-pt":
                                {
                                    SendMessage(selectedProcess.MainWindowHandle, WM_APPCOMMAND, 0, APPCOMMAND_MEDIA_PREVIOUSTRACK);
                                    Console.WriteLine("Message Sent: Previous Track");
                                    break;
                                }
                                case "-?":
                                {
                                    DisplayUsage();
                                    break;
                                }
                                default:
                                {
                                    Console.WriteLine("Ignoring unrecognized command: " + args[argIndex]);
                                    break;
                                }
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine("No process with the name " + args[0] + " is currently running.");
                    }

                    if (volumeControl != null)
                    {
                        Marshal.ReleaseComObject(volumeControl);
                    }
                }
                else
                {
                    DisplayUsage();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception occurred:\r\n" + e.ToString());
            }
        }

        /// <summary>
        /// Displays the command line usage to the console
        /// </summary>
        private static void DisplayUsage()
        {
            Console.WriteLine("Usage: CLMControl <Process Name> [-p] [-pp] [-pa] [-s] [-vm] [-mvm] [-vu] [-mvu] [-vd] [-mvd] [-nt] [-pt] [-?]");
            Console.WriteLine("Parameters:");
            Console.WriteLine("\tProcess Name:\tThe name of the process to send the message to");
            Console.WriteLine("\t-p:\t\tPlay");
            Console.WriteLine("\t-pp:\t\tToggle between play and pause");
            Console.WriteLine("\t-pa:\t\tPause");
            Console.WriteLine("\t-s:\t\tStop");
            Console.WriteLine("\t-vm:\t\tVolume Mute");
            Console.WriteLine("\t-mvm:\t\tMaster Volume Mute");
            Console.WriteLine("\t-vu:\t\tVolume Up");
            Console.WriteLine("\t-mvu:\t\tMaster Volume Up");
            Console.WriteLine("\t-vd:\t\tVolume Down");
            Console.WriteLine("\t-mvd:\t\tMaster Volume Down");
            Console.WriteLine("\t-nt:\t\tNext Track");
            Console.WriteLine("\t-pt:\t\tPrevious Track");
            Console.WriteLine("\r\nNote: All optional parameters may be repeated as necessary");
        }

        /// <summary>
        /// Finds the matching process and volume control
        /// </summary>
        /// <param name="processName">The name of the process to find</param>
        /// <param name="selectedProcess">The selected process</param>
        /// <param name="volumeControl">The volume control object</param>
        private static void FindMatchingProcess(string processName, out Process selectedProcess, out ISimpleAudioVolume volumeControl)
        {
            selectedProcess = null;
            volumeControl = null;

            // First find the list of matching processes
            Process[] matchingProcesses = Process.GetProcessesByName(processName);

            if (matchingProcesses != null && matchingProcesses.Length > 0)
            {
                // Attempt to see if we can find an audio session with the same Process ID as one we have found
                IMMDeviceEnumerator deviceEnumerator = null;
                IMMDevice device = null;
                IAudioSessionManager2 sessionManager = null;
                IAudioSessionEnumerator sessionEnumerator = null;
                object activatedObject = null;
                Guid sessionManagerGuid = typeof(IAudioSessionManager2).GUID;

                try
                {
                    // Create the Device Enumerator
                    deviceEnumerator = (IMMDeviceEnumerator)(new MMDeviceEnumerator());

                    // Get the default audio device
                    if (deviceEnumerator != null)
                    {
                        deviceEnumerator.GetDefaultAudioEndpoint(DataFlow.Render, Role.Multimedia, out device);
                    }

                    // Get the Session Manager
                    if (device != null)
                    {
                        device.Activate(ref sessionManagerGuid, (uint)0, IntPtr.Zero, out activatedObject);
                        sessionManager = activatedObject as IAudioSessionManager2;
                        activatedObject = null;
                    }

                    // Get the Session Enumerator
                    if (sessionManager != null)
                    {
                        sessionManager.GetSessionEnumerator(out sessionEnumerator);
                    }

                    // Look through the list of Sessions and find one with a matching process ID.
                    if (sessionEnumerator != null)
                    {
                        int totalSessions = 0;

                        sessionEnumerator.GetCount(out totalSessions);

                        for (int currentSession = 0; currentSession < totalSessions; currentSession++)
                        {
                            IAudioSessionControl currentSessionControl = null;

                            sessionEnumerator.GetSession(currentSession, out currentSessionControl);

                            IAudioSessionControl2 currentSessionControl2 = currentSessionControl as IAudioSessionControl2;

                            if (currentSessionControl2 != null)
                            {
                                uint processID = 0;

                                currentSessionControl2.GetProcessID(out processID);

                                foreach (Process currentProcess in matchingProcesses)
                                {
                                    // We found the correct process and audio session
                                    if (currentProcess.Id == processID)
                                    {
                                        selectedProcess = currentProcess;
                                        volumeControl = currentSessionControl as ISimpleAudioVolume;
                                        break;
                                    }
                                }
                            }

                            if (selectedProcess != null)
                            {
                                // We found the volume control
                                break;
                            }
                            else
                            {
                                // Free the current session
                                Marshal.ReleaseComObject(currentSessionControl);
                            }
                        }
                    }
                }
                finally
                {
                    // Clean up the COM Objects that we used

                    if (activatedObject != null)
                    {
                        Marshal.ReleaseComObject(activatedObject);
                    }

                    if (sessionEnumerator != null)
                    {
                        Marshal.ReleaseComObject(sessionEnumerator);
                    }

                    if (sessionManager != null)
                    {
                        Marshal.ReleaseComObject(sessionManager);
                    }

                    if (device != null)
                    {
                        Marshal.ReleaseComObject(device);
                    }

                    if (deviceEnumerator != null)
                    {
                        Marshal.ReleaseComObject(deviceEnumerator);
                    }
                }

                // If we get here for some reason and don't have a matching process then we will just take the first one with a valid Window Handle
                if (selectedProcess == null)
                {
                    foreach (Process currentProcess in matchingProcesses)
                    {
                        if (currentProcess != null && currentProcess.MainWindowHandle != IntPtr.Zero)
                        {
                            selectedProcess = currentProcess;
                            break;
                        }
                    }
                }
            }
        }
    }
}
