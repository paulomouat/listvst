using System;
using System.IO;

namespace ListVst
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("List VSTs");

            /* byte onColor = 65;
            byte onColorLimit = 126;
            byte offColor = 1;

            var bytes = File.ReadAllBytes("/Users/paulomouat/Documents/Midi Fighter Twister/cc-toggle-all.mfs");

            var pos = 0;
            while(pos < bytes.Length)
            {
                if (bytes[pos] == 20 && bytes[pos - 2] == 19 && bytes[pos - 4] == 18)
                {
                    bytes[pos - 1] = onColor;
                    bytes[pos + 1] = offColor;
                    if (onColor < onColorLimit)
                        onColor++;
                    offColor++;
                }
                Console.WriteLine(pos);
                pos += 1;
            }

            File.WriteAllBytes("/Users/paulomouat/Documents/Midi Fighter Twister/gradient.mfs", bytes);*/

            Console.WriteLine("Done.");
        }
    }
}
