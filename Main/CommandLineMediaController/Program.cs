/*
 * Command Line Media Player - http://commandlinemedia.codeplex.com
 * Licensed using Ms-PL - http://commandlinemedia.codeplex.com/license
 */

using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Diagnostics;

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
                    string processName = args[0];
                    Process[] matchingProcesses = Process.GetProcessesByName(processName);
                    Process selectedProcess = null;

                    if (matchingProcesses != null && matchingProcesses.Length > 0)
                    {
                        // If there is more than one process take the first one with a valid main window handle
                        foreach(Process currentProcess in matchingProcesses)
                        {
                            if (currentProcess != null && currentProcess.MainWindowHandle != IntPtr.Zero)
                            {
                                selectedProcess = currentProcess;
                                break;
                            }
                        }

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
                                        SendMessage(selectedProcess.MainWindowHandle, WM_APPCOMMAND, 0, APPCOMMAND_VOLUME_MUTE);
                                        Console.WriteLine("Message Sent: Volume Mute");
                                        break;
                                    }
                                    case "-vu":
                                    {
                                        SendMessage(selectedProcess.MainWindowHandle, WM_APPCOMMAND, 0, APPCOMMAND_VOLUME_UP);
                                        Console.WriteLine("Message Sent: Volume Up");
                                        break;
                                    }
                                    case "-vd":
                                    {
                                        SendMessage(selectedProcess.MainWindowHandle, WM_APPCOMMAND, 0, APPCOMMAND_VOLUME_DOWN);
                                        Console.WriteLine("Message Sent: Volume Down");
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
                            Console.WriteLine("Process does not have a valid handle");
                        }
                    }
                    else
                    {
                        Console.WriteLine("No process with the name " + processName + " is currently running.");
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
            Console.WriteLine("Usage: CLMControl <Process Name> [-p] [-pp] [-pa] [-s] [-vm] [-vu] [-vd] [-nt] [-pt] [-?]");
            Console.WriteLine("Parameters:");
            Console.WriteLine("\tProcess Name:\tThe name of the process to send the message to");
            Console.WriteLine("\t-p:\t\tPlay");
            Console.WriteLine("\t-pp:\t\tToggle between play and pause");
            Console.WriteLine("\t-pa:\t\tPause");
            Console.WriteLine("\t-s:\t\tStop");
            Console.WriteLine("\t-vm:\t\tVolume Mute");
            Console.WriteLine("\t-vu:\t\tVolume Up");
            Console.WriteLine("\t-vd:\t\tVolume Down");
            Console.WriteLine("\t-nt:\t\tNext Track");
            Console.WriteLine("\t-pt:\t\tPrevious Track");
            Console.WriteLine("\r\nNote: All optional parameters may be repeated as necessary");
        }
    }
}
