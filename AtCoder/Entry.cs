using System.Diagnostics;

namespace Solve;

public class Entry
{
    public static void Main(string[] args)
    {
        if (!args.Any()) {
            new Solver().Main(Input.Read<In>());
            return;
        }

        var defaultColor = Console.ForegroundColor;
        var sw = new Stopwatch();
        while (true)
        {
            sw.Restart();
            Console.ForegroundColor = defaultColor;
            try
            {
                new Solver().Main(Input.Read<In>());
            }
            catch (FormatException ex)
            {
                var stackTrace = 
                    ex.StackTrace.Split('\n')
                        .Where(x => x.Contains("Program.cs"))
                        .Select(x => x.Replace(@"C:\Users\kichi\RiderProjects\AtCoderCSharp\", ""));
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