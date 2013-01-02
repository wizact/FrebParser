using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.IO;

namespace FrebParserConsole
{
    class Program
    {
        
        static void Main(string[] args)
        {

            if (args.Length == 0)
            {
                Console.WriteLine("Freb Path is required.");
                Console.Read();
                return;
            }

            string path = args[0];

            if (string.IsNullOrEmpty(path))
            {
                Console.WriteLine("Path is invalid.");
                Console.Read();
                return;
            }

            if (!path.EndsWith("\\"))
            {
                path += "\\";
            }

            string[] files = Directory.GetFiles(path, "*.xml");
            int counter = 1;
            foreach (var item in files)
            {
                Console.WriteLine("{0}/{1}", counter, files.Count());
                Console.WriteLine(item);
                (new FrebHelper()).ExtractInfoFromLogFiles(item);
                counter++;
            }
            Console.ReadLine();
        }

        
    }
}
