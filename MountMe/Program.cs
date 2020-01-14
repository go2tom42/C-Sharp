using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MountMe
{
    public class Program
    {
        [STAThreadAttribute]
        public static void Main(string[] args)
        {
            AppDomain.CurrentDomain.AssemblyResolve += OnResolveAssembly;
            
            if (args.Length == 0)
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
