using System;
using System.Diagnostics;
using System.Linq;

namespace Solve
{
    public class Entry
    {
        public static void Main(string[] args)
        {
            if (!args.Any()) {
                new Solver().Main();
                return;
            }

            Console.ForegroundColor = ConsoleColor.White;
            var sw = new Stopwatch();
            while (true)
            {
                sw.Restart();
                Console.ForegroundColor = ConsoleColor.White;
                try
                {
                    new Solver().Main();
                }
                catch (FormatException ex)
                {
                    var stackTrace = 
                        ex.StackTrace.Split('\n')
                            .Where(x => x.Contains("Program.cs"))
                            .Select(x => x.Replace(@"C:\Users\kichi\Documents\develop\AtCoder\LangUpdateCSharp\LangUpdateCSharp\", ""));
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine($"入力に失敗しました:\n  {ex.Message}\n{stackTrace.Join("\n")}");
                }
#if FILE
                return;
#endif
                /*
                Console.ForegroundColor = ConsoleColor.DarkCyan;
                 Console.WriteLine("******** " +
                                  $"{sw.Elapsed.TotalMilliseconds:0.0000} ms" +
                                  " ********");
                */
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine("----------------------------------------------------");
            }
        }
    }
}