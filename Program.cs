using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VSCode_WSL_Launcher
{
    internal class Program
    {
        static void Main(string[] args)
        {

            // Check that WSL exists on system. Exit if not.
            string wslLocation = Path.GetFullPath($"{Environment.SystemDirectory}\\wsl.exe");

            if (!File.Exists(wslLocation))
            {
                 MessageBox.Show($"Could not find wsl.exe in {Environment.SystemDirectory}. \nPlease install WSL to use this application.",
                    "Error",
                    MessageBoxButtons.OK,  MessageBoxIcon.Error);
                Environment.Exit(1);

            }
                 

            var processInfo = new ProcessStartInfo(wslLocation);
            processInfo.CreateNoWindow = false;
            processInfo.UseShellExecute = false;

            // if no args passed, open the program without arguments.
            if (args.Length == 0)
            {
                processInfo.Arguments = ("code");
            }
            else
            {
                // get target file/directory
                string target = args[args.Length - 1];

                // get any flags passed before target
                string flags = "";
                if (args.Length > 1)
                {
                    flags = String.Join(" ", args.Take(args.Length - 1));
                }

                // update working directory and wsl arguments based on if target is directory or file
                if (File.Exists(target))
                {
                    processInfo.WorkingDirectory = Path.GetDirectoryName(target);
                    processInfo.Arguments = ("code " + flags + " " + Path.GetFileName(target));
                }
                else if (Directory.Exists(target))
                {
                    processInfo.WorkingDirectory = Path.GetFullPath(target);
                    processInfo.Arguments = ("code " + flags + " .");
                }

            }

            try
            {
                // start the process with WaitForExit
                using (Process exeProcess = Process.Start(processInfo))
                {
                    exeProcess.WaitForExit();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error Information: " + ex.Message,
                   "Failed to start WSL process.",
                   MessageBoxButtons.OK, MessageBoxIcon.Error);
                                Environment.Exit(1);
            }
        
        }
    }
}
