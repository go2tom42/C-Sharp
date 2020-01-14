using System;
using System.IO;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using System.Diagnostics;

namespace MountMe
{
    internal class App
    {
        internal static void MainBob()
        {
            if (Environment.GetCommandLineArgs()[1].Contains("-mount"))
            {
                //Console.WriteLine("Yes -mount");
                string isoPath = Environment.GetCommandLineArgs()[2];

                using (var ps = PowerShell.Create())
                {

                    //Mount ISO Image
                    var command = ps.AddCommand("Mount-DiskImage");
                    command.AddParameter("ImagePath", isoPath);
                    command.Invoke();
                    ps.Commands.Clear();
                }

            }
            else if (Environment.GetCommandLineArgs()[1].Contains("-unmount"))
            {
                //Console.WriteLine("Yes -unmount");
                string isoPath = Environment.GetCommandLineArgs()[2];

                using (var ps = PowerShell.Create())
                {

                    //Unmount Via Image File Path
                    var command = ps.AddCommand("Dismount-DiskImage");
                    command.AddParameter("ImagePath", isoPath);
                    ps.Invoke();
                    ps.Commands.Clear();
                }

            }
            else if (Environment.GetCommandLineArgs()[1].Contains("-lbox"))
            {
                //Launchbox section
                string isoPath = Environment.GetCommandLineArgs()[2]; //get iso path
                String appPath = Environment.GetCommandLineArgs()[3]; //get app path
                String workingFolder = new FileInfo(appPath).Directory.FullName; //Get directory of app
                Directory.SetCurrentDirectory(workingFolder); //Set the current directory.
                //MOUNT start
                using (var ps = PowerShell.Create())
                {

                    //Mount ISO Image
                    var command = ps.AddCommand("Mount-DiskImage");
                    command.AddParameter("ImagePath", isoPath);
                    command.Invoke();
                    ps.Commands.Clear();
                }
                //MOUNT END

                //Run app
                Process process = new Process();
                          // Configure the process using the StartInfo properties.
                process.StartInfo.FileName = appPath;
                process.StartInfo.WindowStyle = ProcessWindowStyle.Maximized;
                          //Checking if extra args used
                string[] arguments = Environment.GetCommandLineArgs();
                int index = 4;
                if (index < arguments.Length)
                {
                    process.StartInfo.Arguments = Environment.GetCommandLineArgs()[4]; //it exists
                }
                process.Start();
                process.WaitForExit();// Waits here for the process to exit.

                //Unmount Via Image File Path 
                using (var ps = PowerShell.Create())
                {
                    var command = ps.AddCommand("Dismount-DiskImage");
                    command.AddParameter("ImagePath", isoPath);
                    ps.Invoke();
                    ps.Commands.Clear();
                }

            }
            else
            {
                Console.Clear();
                Console.WriteLine();
                Console.WriteLine("You asked for or need help");
                Console.WriteLine("This is a pretty simple program and there isn't much");
                Console.WriteLine("The commands you can submit are");
                Console.WriteLine("     -mount ISOPATH");
                Console.WriteLine("     -unmount ISOPATH");
                Console.WriteLine("     -lbox ISOPATH APPPATH");
                Console.WriteLine("     -lbox ISOPATH APPPATH APPARGUMENTS");
                Console.WriteLine();
                Console.WriteLine("     Examples:");
                Console.WriteLine("     mountme -mount \"c:\\some folder\\file.iso\"");
                Console.WriteLine("     mountme -unmount \"c:\\some folder\\file.iso\"");
                Console.WriteLine("     mountme -lbox \"c:\\some folder\\file.iso\" \"c:\\some folder\\file.exe\"");
                Console.WriteLine("     mountme -lbox \"c:\\some folder\\file.iso\" \"c:\\some folder\\file.exe\" \"-n -i -g\"");
                Console.WriteLine();
                Console.WriteLine("Press any key");
                Console.ReadKey();

            }

        }
    }
}