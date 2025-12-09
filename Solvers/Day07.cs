namespace AoC2025.Solvers;

public class Day07 : BaseSolver
{
    public record Grid((int row, int col) Start, List<(int row, int col)> Splitters);

    public override object Part1()
    {
        var grid = ParseGrid(Input);
        var totalSplits = CountTachyonSplits(grid);
        return totalSplits;
    }
    public override object Part2()
    {
        var grid = Input.Split('\n', StringSplitOptions.RemoveEmptyEntries);
        int height = grid.Length;
        int width = grid[0].Length;

        // Build grid and validate width
        char[][] grid1 = new char[height][];
        for (int y = 0; y < height; y++)
        {
            if (grid[y].Length != width)
                throw new InvalidOperationException("All input lines must have the same length.");

            grid1[y] = grid[y].ToCharArray();
        }

        long[,] dp = new long[height, width];

        // Base case: last row
        for (int x = 0; x < width; x++)
        {
            dp[height - 1, x] = 1;
        }

        // DP bottom-up
        for (int y = height - 2; y >= 0; y--)
        {
            for (int x = 0; x < width; x++)
            {
                char below = grid1[y + 1][x];

                if (below == '^')
                {
                    long total = 0;
                    int ny = y + 1;

                    // Left branch
                    int nx = x - 1;
                    total += (nx >= 0) ? dp[ny, nx] : 1;

                    // Right branch
                    nx = x + 1;
                    total += (nx < width) ? dp[ny, nx] : 1;

                    dp[y, x] = total;
                }
                else
                {
                    dp[y, x] = dp[y + 1, x];
                }
            }
        }

        // Find S
        int startX = -1, startY = -1;
        for (int y = 0; y < height && startX == -1; y++)
        {
            for (int x = 0; x < width; x++)
            {
                if (grid1[y][x] == 'S')
                {
                    startX = x;
                    startY = y;
                    break;
                }
            }
        }

        if (startX == -1)
            throw new InvalidOperationException("No start position 'S' found.");

        return dp[startY, startX];
    }

    /// <summary>
    /// Parses a text grid to extract the start position ('S') and splitter positions ('^')
    /// </summary>
    /// <param name="grid">The text grid as an array of strings</param>
    /// <returns>A Grid object with start position and ordered splitter positions</returns>
    public static Grid ParseGrid(string[] grid)
    {
        (int row, int col) start = (-1, -1);
        var splitters = new List<(int row, int col)>();

        for (int row = 0; row < grid.Length; row++)
        {
            for (int col = 0; col < grid[row].Length; col++)
            {
                char currentChar = grid[row][col];
                if (currentChar == 'S')
                {
                    start = (row, col);
                }
                else if (currentChar == '^')
                {
                    splitters.Add((row, col));
                }
            }
        }

        // Order splitters by row (increasing), then by column
        splitters.Sort((a, b) => a.row != b.row ? a.row.CompareTo(b.row) : a.col.CompareTo(b.col));

        return new Grid(start, splitters);
    }

    /// <summary>
    /// Parses a text grid from a single string to extract the start position ('S') and splitter positions ('^')
    /// </summary>
    /// <param name="gridText">The text grid as a single string with newlines</param>
    /// <returns>A Grid object with start position and ordered splitter positions</returns>
    public static Grid ParseGrid(string gridText)
    {
        var grid = gridText.Split('\n', StringSplitOptions.RemoveEmptyEntries);
        return ParseGrid(grid);
    }

    /// <summary>
    /// Counts the total number of tachyon beam splits in the manifold
    /// </summary>
    /// <param name="grid">The parsed grid with start position and splitters</param>
    /// <returns>The total number of splits that occur</returns>
    public static int CountTachyonSplits(Grid grid)
    {
        // Create a dictionary mapping row -> list of splitter columns for that row
        var splittersByRow = grid.Splitters
            .GroupBy(s => s.row)
            .ToDictionary(g => g.Key, g => g.Select(s => s.col).ToHashSet());
        
        // Find the bounds of the grid
        int maxRow = grid.Splitters.Count > 0 ? grid.Splitters.Max(s => s.row) : grid.Start.row;
        
        // Use a set to track unique beam positions at each step to handle overlapping beams
        var currentBeams = new HashSet<(int row, int col)> { grid.Start };
        
        var totalSplits = 0;
        
        // Process beams row by row
        for (var row = grid.Start.row + 1; row <= maxRow; row++)
        {
            var nextBeams = new HashSet<(int row, int col)>();
            
            // For each current beam position
            foreach (var (beamRow, beamCol) in currentBeams)
            {
                // Check if there's a splitter at this beam's next position
                if (splittersByRow.ContainsKey(row) && splittersByRow[row].Contains(beamCol))
                {
                    // Beam hits a splitter - it splits
                    totalSplits++;
                    
                    // Create two new beams: left and right of the splitter
                    nextBeams.Add((row, beamCol - 1));
                    nextBeams.Add((row, beamCol + 1));
                }
                else
                {
                    // Beam continues downward without hitting a splitter
                    nextBeams.Add((row, beamCol));
                }
            }
            
            // Update current beams for next iteration
            currentBeams = nextBeams;
            
            // If no beams remain, we're done
            if (currentBeams.Count == 0)
            {
                break;
            }
        }
        
        return totalSplits;
    }

}