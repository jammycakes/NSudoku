using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NSudoku.Solving;
using NSudoku.Solving.Solving;

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
                .Add(FishStrategy.XWing)
                .Add(XYWingStrategy.XYWing)
                .Add(FishStrategy.Swordfish)
                .Add(FishStrategy.Jellyfish)
                ;

            Directory.CreateDirectory("solutions");
            using (var resource = File.OpenRead("royle.txt"))
            using (var reader = new StreamReader(resource))
            using (var writer = new StreamWriter("output.txt", false, Encoding.UTF8)) {
                var index = 0;
                var easies = 0;
                writer.WriteLine("Number,Givens,Undetermined,Difficulty,HardestStrategy");
                string line;
                while ((line = await reader.ReadLineAsync()) != null) {
                    line = line.Trim();
                    if (!string.IsNullOrEmpty(line) && !line.StartsWith("#")) {
                        index++;
                        var grid = Grid.Parse(line);
                        var solution = solver.Solve(grid);
                        var unplaced = grid.CountUnplacedDigits();

                        var result =
                            $"{index},{line},{unplaced},{solution.LastStrategyIndex}";
                        if (unplaced == 0) {
                            result += $",{solution.UsedStrategies.LastOrDefault()}";
                        }

                        Console.WriteLine(result);
                        writer.WriteLine(result);

                        var filename = $"solutions\\{index}.txt";
                        using (var file = File.CreateText(filename)) {
                            file.WriteLine(line);
                            file.WriteLine();
                            foreach (var step in solution.Steps) {
                                file.WriteLine(step);
                            }

                            var solved = string.Concat(
                                grid.Select(c =>
                                    (c.Digit.HasValue ? c.Digit.Value.ToString() : ".")));

                            file.WriteLine();
                            file.WriteLine(solved);

                            if (solved.Length != line.Length) {
                                throw new InvalidOperationException($"Solved grid {index} has wrong length");
                            }

                            if (unplaced > 0 && unplaced <= 4) {
                                // This should never happen in a sudoku with a unique solution
                                throw new InvalidOperationException
                                    ($"Grid {index} unexpectedly has four or fewer unplaced candidates.");
                            }
                        }

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
