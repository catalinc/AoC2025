using System.Diagnostics;

namespace AoC2025.Solvers;

public abstract class BaseSolver
{
    public int Day { get; }
    public string Input { get; private set; } = string.Empty;
    public string[] InputLines { get; private set; } = [];

    protected BaseSolver()
    {
        // Parse Day number from Class Name (e.g., "Day01" -> 1)
        Day = int.Parse(GetType().Name.Replace("Day", ""));
        LoadInput();
    }

    private void LoadInput()
    {
        string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Inputs", $"{Day:D2}.txt");
        if (File.Exists(path))
        {
            Input = File.ReadAllText(path).TrimEnd(); // Standard AoC cleanup
            InputLines = File.ReadAllLines(path);
        }
        else
        {
            Console.WriteLine($"[WARNING] Input file not found at: {path}");
        }
    }

    public void Solve(int? part = null)
    {
        Console.WriteLine($"--- Day {Day:D2} ---");

        if (part == null || part == 1)
            RunPart(1, Part1);

        if (part == null || part == 2)
            RunPart(2, Part2);
    }

    private void RunPart(int partNum, Func<object> solverFunction)
    {
        var sw = Stopwatch.StartNew();
        try
        {
            var result = solverFunction();
            sw.Stop();
            Console.WriteLine($"Part {partNum}: {result} ({sw.Elapsed.TotalMilliseconds:F4} ms)");
        }
        catch (NotImplementedException)
        {
            Console.WriteLine($"Part {partNum}: Not Implemented");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Part {partNum}: Error - {ex.ToString()}");
        }
    }

    public abstract object Part1();
    public abstract object Part2();
}