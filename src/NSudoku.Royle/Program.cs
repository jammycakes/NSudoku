using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NSudoku.Solving;
using NSudoku.Strategies;

namespace NSudoku.Royle
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var solver = new Solver()
                .AddSingleStrategies(true)
                .Add(LockedSetStrategy.NakedPair)
                .Add<IntersectionStrategy>()
                .Add(LockedSetStrategy.NakedTriple)
                .Add(LockedSetStrategy.NakedQuad)
                .Add(LockedSetStrategy.HiddenPair)
                .Add(LockedSetStrategy.HiddenTriple)
                .Add(LockedSetStrategy.HiddenQuad)
                ;

            using (var resource = File.OpenRead("royle.txt"))
            using (var reader = new StreamReader(resource))
            using (var writer = new StreamWriter("output.txt", false, Encoding.UTF8)) {
                var index = 0;
                var easies = 0;
                writer.WriteLine("Number,Givens,Undetermined,HardestStrategy");
                string line;
                while ((line = await reader.ReadLineAsync()) != null) {
                    line = line.Trim();
                    if (!string.IsNullOrEmpty(line) && !line.StartsWith("#")) {
                        index++;
                        var grid = Grid.Parse(line);
                        var solution = solver.Solve(grid);
                        var unplaced = grid.CountUnplacedDigits();
                        var result =
                            $"{index},{line},{unplaced}";
                        if (unplaced == 0) {
                            result += $",{solution.UsedStrategies.LastOrDefault()}";
                        }

                        Console.WriteLine(result);
                        writer.WriteLine(result);
                        if (grid.CountUnplacedDigits() == 0) {
                            easies++;
                        }
                    }
                    else {
                        Console.WriteLine(line);
                    }
                }

                Console.WriteLine();
                Console.WriteLine($"# Solved: {easies} Not solved: {index - easies - 1}");
            }
        }
    }
}
