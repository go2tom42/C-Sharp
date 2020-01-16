using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace MountMe
{
    public class Program
    {
        [DllImport("kernel32.dll")]
        static extern IntPtr GetConsoleWindow();

        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        [STAThreadAttribute]
        public static void Main(string[] args)
        {
            const int SW_HIDE = 0;
            const int SW_SHOW = 5;
            var handle = GetConsoleWindow();
            ShowWindow(handle, SW_HIDE); // To hide
            AppDomain.CurrentDomain.AssemblyResolve += OnResolveAssembly;

            if (args.Length == 0)
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
                Environment.Exit(0);
            }
            App.MainBob();
        }

        private static Assembly OnResolveAssembly(object sender, ResolveEventArgs args)
        {
            Assembly executingAssembly = Assembly.GetExecutingAssembly();
            AssemblyName assemblyName = new AssemblyName(args.Name);

            string path = assemblyName.Name + ".dll";
            if (assemblyName.CultureInfo.Equals(CultureInfo.InvariantCulture) == false)
            {
                path = String.Format(@"{0}\{1}", assemblyName.CultureInfo, path);
            }

            using (Stream stream = executingAssembly.GetManifestResourceStream(path))
            {
                if (stream == null)
                    return null;

                byte[] assemblyRawBytes = new byte[stream.Length];
                stream.Read(assemblyRawBytes, 0, assemblyRawBytes.Length);
                return Assembly.Load(assemblyRawBytes);
            }
        }
    }
}
