namespace AoC2025.Solvers;

public class Day04: BaseSolver
{
    private static readonly int[] Directions = [-1, 0, 1];

    public override object Part1()
    {
        var lines = InputLines;
        var map = ParseMap(lines);

        var total = 0;

        for (var i = 0; i < map.GetLength(0); i++)
        {
            for (var j = 0; j < map.GetLength(1); j++)
            {
                if (map[i, j] != 1) continue;
                var numRolls = (from k in Directions from t in Directions where k != 0 || t != 0 let r = i + k let c = j + t where r >= 0 && r < map.GetLength(0) && c >= 0 && c < map.GetLength(1) select map[r, c]).Sum();
                if (numRolls < 4) total++;
            }
        }

        return total;
    }

    private static int[,] ParseMap(string[] lines)
    {
        var map = new int[lines.Length, lines[0].Length];

        for (var i = 0; i < lines.Length; i++)
        {
            var line = lines[i];
            for (var j = 0; j < line.Length; j++)
            {
                map[i, j] = line[j] == '@' ? 1 : 0;
            }
        }

        return map;
    }

    public override object Part2()
    {
        var lines = InputLines;
        var map = ParseMap(lines);

        var totalRemoved = 0;
        while (true)
        {
            var bRemoved = false;
            for (var i = 0; i < map.GetLength(0); i++)
            {
                for (var j = 0; j < map.GetLength(1); j++)
                {
                    if (map[i, j] != 1) continue;
                    var numRolls = (from k in Directions from t in Directions where k != 0 || t != 0 let r = i + k let c = j + t where r >= 0 && r < map.GetLength(0) && c >= 0 && c < map.GetLength(1) select map[r, c]).Sum();
                    if (numRolls >= 4) continue;
                    map[i,j] = 0;
                    totalRemoved++;
                    bRemoved = true;
                }
            }
            if (!bRemoved) break;
        }

        return totalRemoved;
    }
}