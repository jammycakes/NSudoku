using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;

namespace NSudoku.Tests.Strategies
{
    public class GridLoader
    {
        public static Grid Load(IEnumerable<string> input)
        {
            var lines = input.Where(line => !string.IsNullOrEmpty(line)).ToArray();
            var grid = new Grid((byte)lines.Length);
            for (byte row = 1; row <= 9; row++) {
                var line = Regex.Split(lines[row - 1].Trim(), @"\s+");

                for (byte col = 1; col <= 9; col++) {
                    var data = line[col - 1];
                    var cell = grid[row, col ];
                    if (data.Length == 1) {
                        cell.PlaceDigit(byte.Parse(data));
                    }
                    else {
                        foreach (var digit in data) {
                            cell.Candidates.Add(byte.Parse(digit.ToString()));
                        }
                    }
                }
            }
            return grid;
        }

        public static Grid Load(string name)
        {
            var type = typeof(GridLoader);
            var resourceName = type.Namespace + ".TestGrids." + name;
            using var resource = type.Assembly.GetManifestResourceStream(resourceName);
            using var reader = new System.IO.StreamReader(resource, Encoding.UTF8);
            return Load(reader.ReadToEnd().Split('\n'));
        }
    }
}
