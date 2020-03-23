using System;
using ArticyProjectJsonParser.Core;

namespace ArticyProjectJsonParser
{
    class Program
    {
        static void Main(string[] args)
        {
            var jsonPath = args[0];
            var sqliteDatabasePath = args[1];

            var parser = new Parser();
            var result = parser.Parse(jsonPath);

            var saver = new Saver();
            saver.Save(result);

            Console.WriteLine("Done!");
        }
    }
}
