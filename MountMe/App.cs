using System;
using System.IO;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using System.Diagnostics;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace MountMe
{
    internal class App
    {
        [DllImport("kernel32.dll")]
        static extern IntPtr GetConsoleWindow();

        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        internal static void MainBob()
        {
            const int SW_HIDE = 0;
            const int SW_SHOW = 5;
            var handle = GetConsoleWindow();
            ShowWindow(handle, SW_HIDE); // To hide
            bool boxExists = false;
            string lboxpath = null;
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

                //Get Launchbox Path

                //Get command line args and fix if needed
                string isoPath = Environment.GetCommandLineArgs()[2]; //get iso path
                string appPath = Environment.GetCommandLineArgs()[3]; //get app path
                //Check if relative path 
                if (!isoPath.Contains(":"))
                {
                    GetLaunchBoxPath();
                    //Console.WriteLine(lboxpath);
                    boxExists = !string.IsNullOrEmpty(lboxpath);
                    ExitIfRelativePathAndNoLaunchBox();
                    if (isoPath.StartsWith("\\"))
                    {
                        isoPath = isoPath.Substring(1);
                    }
                    isoPath = Path.Combine(lboxpath, isoPath);
                    //Console.WriteLine(isoPath);
                }

                if (!appPath.Contains(":"))
                {
                    GetLaunchBoxPath();
                    //Console.WriteLine(lboxpath);
                    boxExists = !string.IsNullOrEmpty(lboxpath);
                    ExitIfRelativePathAndNoLaunchBox();
                    if (appPath.StartsWith("\\"))
                    {
                        appPath = appPath.Substring(1);
                    }
                    appPath = Path.Combine(lboxpath, appPath);
                    //Console.WriteLine(appPath);
                }


                //Get folder for app and set as working folder
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
                process.StartInfo.FileName = appPath;
                process.StartInfo.WindowStyle = ProcessWindowStyle.Maximized;


                //Checking if extra args used
                string[] arguments = Environment.GetCommandLineArgs();
                int index = 4;
                if (index < arguments.Length)
                {
                    if (Environment.GetCommandLineArgs()[4].Contains("%DRIVE%"))
                    {
                        string driveLetter = null;
                        using (var ps = PowerShell.Create())
                        {
                            //Get Drive Letter ISO Image Was Mounted To
                            var runSpace = ps.Runspace;
                            var pipeLine = runSpace.CreatePipeline();
                            var getImageCommand = new Command("Get-DiskImage");
                            getImageCommand.Parameters.Add("ImagePath", isoPath);
                            pipeLine.Commands.Add(getImageCommand);
                            pipeLine.Commands.Add("Get-Volume");


                            foreach (PSObject psObject in pipeLine.Invoke())
                            {
                                driveLetter = psObject.Members["DriveLetter"].Value.ToString();
                                //Console.WriteLine("Mounted On Drive: " + driveLetter);
                            }
                            pipeLine.Commands.Clear();

                        }
                        //Console.WriteLine("Mounted On Drive: " + driveLetter);
                        string tempargs = Environment.GetCommandLineArgs()[4]; //it exists
                        process.StartInfo.Arguments = tempargs.Replace("%DRIVE%", driveLetter);

                    }
                    else if (Environment.GetCommandLineArgs()[4].Contains("%DRIVE:%"))
                    {
                        string driveLetter = null;
                        using (var ps = PowerShell.Create())
                        {
                            //Get Drive Letter ISO Image Was Mounted To
                            var runSpace = ps.Runspace;
                            var pipeLine = runSpace.CreatePipeline();
                            var getImageCommand = new Command("Get-DiskImage");
                            getImageCommand.Parameters.Add("ImagePath", isoPath);
                            pipeLine.Commands.Add(getImageCommand);
                            pipeLine.Commands.Add("Get-Volume");


                            foreach (PSObject psObject in pipeLine.Invoke())
                            {
                                driveLetter = psObject.Members["DriveLetter"].Value.ToString();
                                //Console.WriteLine("Mounted On Drive: " + driveLetter);
                            }
                            pipeLine.Commands.Clear();

                        }
                        //Console.WriteLine("Mounted On Drive: " + driveLetter);
                        string tempargs = Environment.GetCommandLineArgs()[4]; //it exists
                        process.StartInfo.Arguments = tempargs.Replace("%DRIVE:%", driveLetter + ":");
                    }
                    
                    
                    else 
                    {
                        process.StartInfo.Arguments = Environment.GetCommandLineArgs()[4]; //it exists
                    }
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
                ShowWindow(handle, SW_SHOW); // To show
                Console.Clear();
                Console.WriteLine();
                Console.WriteLine("You asked for or needed help");
                Console.WriteLine("This is a pretty simple program and there isn't much");
                Console.WriteLine("The commands you can submit are");
                Console.WriteLine("     -mount ISOPATH");
                Console.WriteLine("     -unmount ISOPATH");
                Console.WriteLine("     -lbox ISOPATH APPPATH");
                Console.WriteLine("     -lbox ISOPATH APPPATH APPARGUMENTS");
                Console.WriteLine("     If you use %DRIVE% or %DRIVE:% (note the colon) in APPARGUMENTS it's replaced with the drive letter of mounted ISO");
                Console.WriteLine();
                Console.WriteLine("     Examples:");
                Console.WriteLine("     mountme -mount \"c:\\some folder\\file.iso\"");
                Console.WriteLine("     mountme -unmount \"c:\\some folder\\file.iso\"");
                Console.WriteLine("     mountme -lbox \"c:\\some folder\\file.iso\" \"c:\\some folder\\file.exe\"");
                Console.WriteLine("     mountme -lbox \"c:\\some folder\\file.iso\" \"c:\\some folder\\file.exe\" \"-d %DRIVE:% -Fullscreen\"");
                Console.WriteLine("                 After mounted App run like \"c:\\some folder\\file.exe\" \"-d G: -Fullscreen\"");
                Console.WriteLine("     mountme -lbox \"c:\\some folder\\file.iso\" \"c:\\some folder\\file.exe\" \"%DRIVE:%\\PS3_GAME\\USRDIR\\EBOOT.BIN\"");
                Console.WriteLine("                 After mounted App run like \"c:\\some folder\\file.exe\" \"G:\\PS3_GAME\\USRDIR\\EBOOT.BIN\"");
                Console.WriteLine();
                Console.WriteLine("Press any key");
                Console.ReadKey();

            }
            void GetLaunchBoxPath()
            {
                var procList = Process.GetProcessesByName("LaunchBox");
                foreach (Process instance in procList)
                {
                    try
                    {
                        lboxpath = instance.MainModule.FileName;
                        lboxpath = lboxpath.Replace("\\LaunchBox.exe","");

                    }
                    catch (Win32Exception w32ex)
                    {
                        Console.WriteLine(w32ex.Message);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
                bool launchboxExists = !string.IsNullOrEmpty(lboxpath);
                if (!launchboxExists)
                {
                    procList = Process.GetProcessesByName("BigBox");
                    foreach (Process instance in procList)
                    {
                        try
                        {
                            lboxpath = instance.MainModule.FileName;
                            lboxpath = lboxpath.Replace("\\BigBox.exe", "");

                        }
                        catch (Win32Exception w32ex)
                        {
                            Console.WriteLine(w32ex.Message);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                }

            }
            void ExitIfRelativePathAndNoLaunchBox()
            {
                if (!boxExists)
                {
                    Console.Clear();
                    Console.WriteLine("Niether LaunchBox nor BigBox is running, Bye");
                    Console.WriteLine("Press any key");
                    Console.ReadKey();
                    Environment.Exit(0);
                }
            }
        }
    }
}