using System;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
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
            else
            {
                //Console.WriteLine($"No {Environment.GetCommandLineArgs()[1]}");
            }

        }
    }
}